using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLibrary
{
    public class Method
    {
        public string Signature { get; set; }

        public Method(MethodInfo methodinfo)
        {
            String[] methodParams = methodinfo.GetParameters().Select(p => String.Format("{0} {1}", p.ParameterType.Name, p.Name)).ToArray();

            string AccessModifier = "";
            string Modifier = "";

            MethodAttributes attr = methodinfo.Attributes;

            if (attr.HasFlag(MethodAttributes.Public))
            {
                AccessModifier = "public";
            }
            else if (attr.HasFlag(MethodAttributes.Private))
            {
                AccessModifier = "private";
            }
            else if (attr.HasFlag(MethodAttributes.Family))
            {
                AccessModifier = "protected";
            }
            else if (attr.HasFlag(MethodAttributes.Assembly))
            {
                AccessModifier = "internal";
            }

            if (attr.HasFlag(MethodAttributes.Abstract))
            {
                Modifier = "abstract";
            }
            else if (attr.HasFlag(MethodAttributes.Virtual))
            {
                Modifier = "virtual";
            }
            else if (attr.HasFlag(MethodAttributes.Static))
            {
                Modifier = "static";
            }

            Signature = String.Format("{0} {1} {2} {3} ({4})", AccessModifier, Modifier, methodinfo.ReturnType.Name, methodinfo.Name, String.Join(",", methodParams));
        }
    }
}
