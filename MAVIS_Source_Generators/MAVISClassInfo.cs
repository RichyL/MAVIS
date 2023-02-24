using System.Text;

namespace MAVIS_Source_Generators
{
    public class MAVISClassInfo
    {
        public string ClassName { get; }
        public string NameSpace { get;  }
        public List<MAVISPropertyInformation>? Properties { get; }

        public List<MAVISMethodInformation>? Methods{ get; }

        public MAVISClassInfo(string className,string nameSpace, List<MAVISPropertyInformation> properties)
        {
            ClassName=className;
            NameSpace=nameSpace;    
            Properties=properties;
        }

        public MAVISClassInfo(string className, string nameSpace, List<MAVISMethodInformation> methods)
        {
            ClassName = className;
            NameSpace = nameSpace;
            Methods = methods;
        }

    }

    public class MAVISMethodInformation
    {
        public string? MethodName { get; set; }

        public bool HasExecuteMethod { get; set; } = false;

        public bool MethodHasArgument { get; set; } = false;

        public bool CanExecuteHasArgument { get; set; } = false;

        public string CapitilizedMethodName()
        {
            if (MethodName.Length < 2) throw new ArgumentException($"The variable name {MethodName} is too short to capitalize");
            return char.ToUpper(MethodName[0]) + MethodName.Substring(1);
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder(@"            predicate = (object o) => {");

            if (HasExecuteMethod)
            {
                if (CanExecuteHasArgument)
                {
                    stringBuilder.Append($"          return CanExecute{MethodName}(o);");
                }
                else
                {
                    stringBuilder.Append($"          return CanExecute{MethodName}();");
                }

            }
            else
            {
                stringBuilder.Append($"           return true;");

            }

            stringBuilder.Append("};");

            stringBuilder.AppendLine();

            stringBuilder.Append(@"            command = ");
            if (MethodHasArgument)
            {
                stringBuilder.Append($"{MethodName};");
            }
            else
            {
                stringBuilder.Append(@$"(object o) => {{ {MethodName}();}};");
            }


            return stringBuilder.ToString();
        }

    }



    public class MAVISPropertyInformation
    {
        public string? PropertyName { get; set; }
        public string? PropertyType { get; set; }

        private string CapitilizedPropertyName()
        {
            if (PropertyName.Length < 2) throw new ArgumentException($"The variable name {PropertyName} is too short to capitalize");
            return char.ToUpper(PropertyName[0]) + PropertyName.Substring(1);
        }

        public override string ToString()
        {
            return $@" public {PropertyType} {CapitilizedPropertyName()}Property {{
                        get {{ return {PropertyName}; }}
                        set {{ if (value != {PropertyName}) {{
                                {PropertyName} = value;
                                NotifyPropertyChanged(nameof({CapitilizedPropertyName()}Property));
                            }}
                        }}
                    }}";
        }

    }
}
