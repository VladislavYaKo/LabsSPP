using Microsoft.VisualBasic.FileIO;
using System;
using System.Reflection;

namespace AssemblyBrowserLibrary
{
    public class Field
    {
        public string Signature { get; set; }

        public Field(FieldInfo fieldInfo)
        {
            FieldAttributes attributes = fieldInfo.Attributes;

            if (attributes.HasFlag(FieldAttributes.Public))
                Signature = "public ";
            else if (attributes.HasFlag(FieldAttributes.Private))
            {
                Signature = "private ";
            }
            else if (attributes.HasFlag(FieldAttributes.Family))
            {
                Signature = "protected ";
            }
            else if (attributes.HasFlag(FieldAttributes.Assembly))
            {
                Signature = "internal ";
            }

            if (attributes.HasFlag(FieldAttributes.Static))
            {
                Signature = Signature + "static ";
            }
            else if (attributes.HasFlag(FieldAttributes.InitOnly))
            {
                Signature = Signature + "readonly ";
            }

            Signature = Signature + fieldInfo.FieldType.Name + " " + fieldInfo.Name;
        }
    }
}
