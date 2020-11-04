using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLibrary
{
    public class Constructor
    {
        public string Signature { get; set; }

        public Constructor(ConstructorInfo constructorInfo)
        {
            string AccessModifier = "";

            if (constructorInfo.IsPublic)
            {
                AccessModifier = "public";
            }
            else if (constructorInfo.IsPrivate)
            {
                AccessModifier = "private";
            }
            else if (constructorInfo.IsFamily)
            {
                AccessModifier = "protected";
            }
            else if (constructorInfo.IsAssembly)
            {
                AccessModifier = "internal";
            }

            Signature = AccessModifier + " " + constructorInfo.DeclaringType.Name + "(";

            ParameterInfo[] parameterInfos = constructorInfo.GetParameters();
            for (int i = 0; i < parameterInfos.Length; i++)
            {
                Signature = Signature + parameterInfos[i].ParameterType.Name + " " + parameterInfos[i].Name;
                if (i + 1 < parameterInfos.Length)
                {
                    Signature = Signature + ", ";
                }
            }

            Signature = Signature + ")";
        }
    }
}
