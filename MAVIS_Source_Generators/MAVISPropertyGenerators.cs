using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace MAVIS_Source_Generators
{

    [Generator(LanguageNames.CSharp)]
    public class MAVISPropertyGenerator : IIncrementalGenerator
    {
        const string SimpleMAVISPropertyIdentifier = "[MAVIS_Property]";
        const string FullMAVISPropertyIdentifier = "[Richy_WPF_MVVM.Common.MAVIS_Property]";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Find MAVIS_Property attributes
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

            
//#if DEBUG
//            if (!Debugger.IsAttached)
//            {
//                Debugger.Launch();
//            }
//#endif


            foreach (var item in classSyntax.Members)
            {
                if (item is FieldDeclarationSyntax && item.AttributeLists.FirstOrDefault(x => x.ToString() == SimpleMAVISPropertyIdentifier) !=null) return true;
                continue;
            }
            return false;


        }

        private static MAVISClassInfo Transform(GeneratorSyntaxContext context, CancellationToken cancellation)
        {


            //this cast is superfluous in one sense as the predicate called earlier should have selected the classes where 
            //the MAVIS_Property is used
            var classSyntax = context.Node as ClassDeclarationSyntax;

            //this will be the list of all the properties annotated with MAVIS_Property
            var properties = new List<MAVISPropertyInformation>();

            //iterate through all the members of the class - it'll include methods and properties
            foreach (var member in classSyntax.Members)
            {
                
                //if the member is not a field then ignore
                if (member is not FieldDeclarationSyntax methodSyntax) continue;

                //it is a field but there is no annotation then ignore
                if (!member.AttributeLists.Any()) continue;

                if (member is not FieldDeclarationSyntax targetField) continue;

                //a field declaration can have more than one variable as seen in the following code
                //
                // int a , b = 3;
                //
                //therefore it is necessary to iterate through possible declarations
                //TODO - is the following handled
                //
                // [MAVIS_Property]
                // int a , b = 3;
                foreach (var variable in targetField.Declaration.Variables)
                {
                    //this extracts the semantic model for the class member
                    //the best way to consider the semantic model is by the type of questions it can answer
                    //whereas the syntatic model can answer questions such as "What are the child nodes?", what triva (whitespace etc.) is present
                    //the semantic model answers questions like "What is the name of the variable name?", "What attributes does it have?"
                    var variableSymbol = context.SemanticModel.Compilation.GetSemanticModel(member.SyntaxTree).GetDeclaredSymbol(variable);

                    if (variableSymbol is not IFieldSymbol fieldSymbol)continue;

                    //get all attributes for the variable
                    var variableAttributes = variableSymbol.GetAttributes();

                    //iterate through each attribute (this equals annotation) on the property
                    foreach (var attribute in variableAttributes)
                    {
                        //the attribute does not include the MAVIS_Property then move on to the next
                        if (!FullMAVISPropertyIdentifier.Contains(attribute.AttributeClass.ToString())) continue;

                        //store the MAVIS_Property information of interest
                        //which will be name and Type
                        var mavisProperty = new MAVISPropertyInformation()
                        {
                            PropertyName = variableSymbol.Name,
                            PropertyType = fieldSymbol.Type.ToString()
                        };

                        //add to list of properties
                        properties.Add(mavisProperty);
                    }

                }
            }

            //if no properties are detected then stop here - this should never occur as
            //predicate should only select classes where a MAVIS_Property is used
            if (!properties.Any()) return null;

            var className = classSyntax.Identifier.ToString();

            var namespaceSyntax = classSyntax.Parent as NamespaceDeclarationSyntax;
            var nameSpace = namespaceSyntax.Name.ToString();

            return new MAVISClassInfo(className,nameSpace,properties);
        }

          private void GenerateOutput(SourceProductionContext context, ImmutableArray<MAVISClassInfo> result)
        {
            foreach (var item in result)
            {

                var className = $"{item.ClassName}_Properties.g.cs";
                StringBuilder sb = new StringBuilder();
                sb.Append($@"// <auto-generated/>

using Richy_WPF_MVVM.Common;

namespace {item.NameSpace}
{{

    public partial class {item.ClassName} : ViewModelBase
    {{");

                foreach(var property in item.Properties)
                {
                    sb.AppendLine();
                    sb.AppendLine();
                    sb.Append("\t\t");
                    sb.Append(property.ToString());
                    sb.AppendLine();
                    sb.AppendLine();
                }    

        sb.Append($@"
    }}
}}");

                var classSourceCode = SourceText.From(sb.ToString(), Encoding.UTF8);


                context.AddSource(className, classSourceCode);
            }
        }

        static string PROPERTY_TEMPLATE = @"
// <auto-generated/>

using Richy_WPF_MVVM.Common;

namespace {{namespace}}
{

    public partial class {{classname}} : ViewModelBase
    {

		{{#properties}}
        {{/properties}}

    }
}";

    }

}
