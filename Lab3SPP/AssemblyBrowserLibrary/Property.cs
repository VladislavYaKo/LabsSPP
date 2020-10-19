using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace AssemblyBrowserLibrary
{
    public class Property
    {
        public string Signature { get; set; }

        public Property(PropertyInfo propertyInfo)
        {
            string AccessModifier = "";

            if (propertyInfo.PropertyType.IsPublic)
            {
                AccessModifier = "public";
            }
            else if (propertyInfo.PropertyType.IsNotPublic)
            {
                AccessModifier = "private";
            }
            else if (propertyInfo.PropertyType.IsNestedFamily)
            {
                AccessModifier = "protected";
            }
            else if (propertyInfo.PropertyType.IsNestedAssembly)
            {
                AccessModifier = "internal";
            }

            string getter = "";
            var getmethod = propertyInfo.GetGetMethod(true);
            if (getmethod != null)
            {
                getter = " get; ";
                if (getmethod.IsPrivate)
                {
                    getter = " private get; ";
                }
            }

            string setter = "";
            var setmethod = propertyInfo.GetSetMethod(true);
            if (setmethod != null)
            {
                setter = " set; ";
                if (setmethod.IsPrivate)
                {
                    setter = " private set; ";
                }
            }

            Signature = AccessModifier + " " + propertyInfo.PropertyType.Name + " " + propertyInfo.Name + "{" + getter + setter + "}";
        }
    }
}
