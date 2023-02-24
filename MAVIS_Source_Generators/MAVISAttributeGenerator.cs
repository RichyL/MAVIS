namespace MAVIS_Source_Generators
{
    [Generator]
    public class MAVISAttributeGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var hint = "MAVISAttributes.g.cs";
            var markerSource = @"
using System;

namespace Richy_WPF_MVVM.Common
{
    public class MAVIS_Property : Attribute
    {
    }

    public class MAVIS_Method : Attribute
    {
    }
}";

            context.RegisterPostInitializationOutput(ctx => { ctx.AddSource(hint, markerSource); });
        }
    }
}
