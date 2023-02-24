using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace MAVIS_Source_Generators
{

    [Generator(LanguageNames.CSharp)]
    public class MAVISMethodGenerator : IIncrementalGenerator
    {
        const string SimpleMAVISMethodIdentifier = "[MAVIS_Method]";
        const string FullMAVISMethodIdentifier = "[Richy_WPF_MVVM.Common.MAVIS_Method]";
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Find MAVIS_Method attributes
            //predicate = filter used to decide if the node passed to CreateSyntaxProvider is of interestest to this source generator
            //transform = semantic information related to the node which satisfied the terms of the predicate
            var classesWithAttribute= context.SyntaxProvider.CreateSyntaxProvider(
                predicate: Predicate,
                transform: Transform).Where(x => x != null)
                .Collect();


            context.RegisterSourceOutput(classesWithAttribute, GenerateOutput);

        }

        /// <summary>
        /// Tests to see if a given node is a partial class but not a partial static class.
        /// Note this class may not have the attributes which are of interest to this source generator but the aim here is to reduce the work 
        /// the source generator does by filtering down the nodes which it will need to examine
        /// </summary>
        /// <param name="syntaxNode">Compiliation node passed by compiler</param>
        /// <param name="cancellationToken"></param>
        /// <returns>True if syntaxNode represents a partial class declaration and false otherwise</returns>
        private static bool Predicate(SyntaxNode syntaxNode, CancellationToken cancellationToken)
        {
            if (syntaxNode is not ClassDeclarationSyntax classSyntax) return false;
            

            foreach (var item in classSyntax.Members)
            {
                if (item is MethodDeclarationSyntax && item.AttributeLists.FirstOrDefault(x => x.ToString() == SimpleMAVISMethodIdentifier) !=null) return true;
                continue;
            }
            return false;


        }

        private static MAVISClassInfo Transform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {

            
            //this cast is superfluous in one sense as the predicate called earlier should have selected the classes where 
            //the MAVIS_Property is used
            var classSyntax = context.Node as ClassDeclarationSyntax;

            //this will be the list of all the methods annotated with MAVIS_Method
            var mavis_methods = new List<MAVISMethodInformation>();

            var non_mavis_methods=new List<NonMavisMethod>();

            //iterate through all the members of the class - it'll include methods and properties
            foreach (var member in classSyntax.Members)
            {
                
                //if the member is not a method then ignore
                if (member is not MethodDeclarationSyntax methodSyntax) continue;

                //it is a method but there is no annotation then ignore
                //if (!member.AttributeLists.Any()) continue;


                //get the semantic model of the method
                var methodSemanticModel = context.SemanticModel.Compilation.GetSemanticModel(member.SyntaxTree);

                if (methodSemanticModel.GetDeclaredSymbol(member) is not IMethodSymbol methodSymbol) continue;

                //get attributes on the method
                var memberAttributes = methodSymbol.GetAttributes();

                bool _isMavisMethod = false;
                foreach (var attribute in memberAttributes)
                {
                    //if the attribute is not MAVIS_Method then ignore
                    if (!FullMAVISMethodIdentifier.Contains(attribute.AttributeClass.ToString())) continue;

                    //at this point found a MAVIS_Method
                    mavis_methods.Add( new MAVISMethodInformation { MethodName=methodSymbol.Name , MethodHasArgument= methodSymbol.Parameters.Count() > 0});
                    _isMavisMethod = true;
                    break;
                }

                if (!_isMavisMethod)
                {
                    non_mavis_methods.Add(new NonMavisMethod(methodSymbol.Name, methodSymbol.Parameters.Count()>0));
                }
            }

            //if no methods are detected then stop here - this should never occur as
            //predicate should only select classes where a MAVIS_Method is used
            if (!mavis_methods.Any()) return null;

            //need to get the CanExecute methods for any MAVIS_Methods if they exist
            foreach(var mavisMethod in mavis_methods)
            {
                var canExecuteMethod = non_mavis_methods.FirstOrDefault(x => x.Name.Contains($"CanExecute{mavisMethod.MethodName}"));
                if ( canExecuteMethod != null ) 
                {
                    mavisMethod.HasExecuteMethod = true;
                    mavisMethod.CanExecuteHasArgument = canExecuteMethod.HasArguments;
                }
            }

            var className = classSyntax.Identifier.ToString();

            var namespaceSyntax = classSyntax.Parent as NamespaceDeclarationSyntax;
            var nameSpace = namespaceSyntax.Name.ToString();

            return new MAVISClassInfo(className,nameSpace,mavis_methods);
        }

        private void GenerateOutput(SourceProductionContext context, ImmutableArray<MAVISClassInfo> result)
        {
            foreach (var item in result)
            {

                var className = $"{item.ClassName}_Methods.g.cs";
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($@"// <auto-generated/>


using Richy_WPF_MVVM.Common;
using System.Windows;
using System.Windows.Input;
using System;


namespace {item.NameSpace}
{{

    public partial class {item.ClassName} : ViewModelBase
    {{");

                //add the public declarations of the variables
                foreach (var method in item.Methods)
                {
                    sb.AppendLine(@$"       public ICommand {method.CapitilizedMethodName()}Command {{ get; private set; }}");
                }

                sb.AppendLine();
                sb.AppendLine("");
                sb.AppendLine("     private void InitiateMAVISCommands()");
                sb.AppendLine("     {");
                sb.AppendLine("         Func<object, bool> predicate;");
                sb.AppendLine("         Action<object> command;");


                foreach (var method in item.Methods)
                    {
                        sb.AppendLine();
                        sb.AppendLine($"            // Definition for  {method.CapitilizedMethodName()} -- START");
                        sb.Append(method.ToString());
                        sb.AppendLine();
                        sb.AppendLine($@"              {method.CapitilizedMethodName()}Command = new GenericCommand( canExec: predicate, doExec: command);");
                        sb.AppendLine($"            // Definition for  {method.CapitilizedMethodName()} -- END");
                }

                sb.AppendLine("     }");


                sb.Append($@"
    }}
}}");

                var classSourceCode = SourceText.From(sb.ToString(), Encoding.UTF8);


                context.AddSource(className, classSourceCode);
            }

            
        }

        private class NonMavisMethod
        {
            public string Name { get; }
            public bool HasArguments { get; }

            public NonMavisMethod(string name, bool hasArguments)
            {
                Name = name;
                HasArguments = hasArguments;
            }
        }


        public static string METHOD_TEMPLATE = @"
// <auto-generated/>

using System;
using Richy_WPF_MVVM.Common;
using System.Windows.Input;

namespace  {{NameSpace}}
{
    public partial class {{ClassName}} : ViewModelBase
    {
        {{#Methods}}
        public ICommand {{MethodName}}Command { get; private set; }
        {{/Methods}}

        private void InitiateMAVISCommands()
        {
            
            Func<object, bool> predicate;
            Action<object> command;

            //------------ Code for {{MethodName}} - START
            {{#Methods}}
            Func<object, bool> predicate = (object o) => { return {{#HasExecuteMethod}} CanExecute{{MethodName}}{{#CanExecuteHasArgument}}(o){{/CanExecuteHasArgument}}{{^CanExecuteHasArgument}}();{{/CanExecuteHasArgument}}{{/HasExecuteMethod}}{{^HasExecuteMethod}}true;{{/HasExecuteMethod}} };
                                                                        
            command =  {{MethodName}}{{#MethodHasArgument}}(o){{/MethodHasArgument}}{{^MethodHasArgument}}(){{/MethodHasArgument}};

            {{MethodName}}Command = new GenericCommand( canExec: predicate, doExec: command);
            
            //------------ Code for {{MethodName}} - END

            {{/Methods}}

        }


    }
}";
    }

}
