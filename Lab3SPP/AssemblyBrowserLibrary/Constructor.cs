using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

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

            Signature = AccessModifier + " " + constructorInfo.Name + "(";

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
