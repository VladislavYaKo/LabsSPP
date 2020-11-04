using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace AssemblyBrowserLibrary
{
    public class Class
    {
        public string Name { get; set; }
        public List<Method> Methods { get; set; }
        public List<Field> Fields { get; set; }
        public List<Property> Properties { get; set; }
        public List<Constructor> Constructors { get; set; }

        public ICollection Collections
        {
            get
            {
                return new CompositeCollection()
                {
                    new CollectionContainer(){ Collection = Methods },
                    new CollectionContainer(){ Collection = Fields },
                    new CollectionContainer(){ Collection = Properties },
                    new CollectionContainer(){ Collection = Constructors }
                };
            }
        }

        public Class(Type type)
        {
            TypeAttributes attributes = type.Attributes;

            if (attributes.HasFlag(TypeAttributes.Public))
            {
                Name = "public ";
            }
            else if (attributes.HasFlag(TypeAttributes.NotPublic))
            {
                Name = "private ";
            }
            else if (attributes.HasFlag(TypeAttributes.NestedFamily))
            {
                Name = "protected ";
            }
            else if (attributes.HasFlag(TypeAttributes.NestedAssembly))
            {
                Name = "internal ";
            }

            if (attributes.HasFlag(TypeAttributes.Interface))
            {
                Name = Name + "interface ";
            }
            else if (attributes.HasFlag(TypeAttributes.Class))
            {
                Name = Name + "class ";
            }

            Name = Name + type.Name;
            Methods = new List<Method>();

            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            Methods = GetMethods(type, bindingFlags);
            Fields = GetFields(type, bindingFlags);
            Properties = GetProperties(type, bindingFlags);
            Constructors = GetConstructors(type, bindingFlags);
        }

        public List<Method> GetMethods(Type type, BindingFlags bindingFlags)
        {
            MethodInfo[] methods = type.GetMethods(bindingFlags);
            List<Method> tmp = new List<Method>();
            foreach (MethodInfo methodInfo in methods)
            {
                if (!methodInfo.IsSpecialName && methodInfo.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                {
                    Method method = new Method(methodInfo);
                    tmp.Add(method);
                }
            }
            return tmp;
        }

        public List<Field> GetFields(Type type, BindingFlags bindingFlags)
        {
            FieldInfo[] fields = type.GetFields(bindingFlags);
            List<Field> tmp = new List<Field>();
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                {
                    Field field = new Field(fieldInfo);
                    tmp.Add(field);
                }
            }
            return tmp;
        }

        public List<Property> GetProperties(Type type, BindingFlags bindingFlags)
        {
            PropertyInfo[] properties = type.GetProperties(bindingFlags);
            List<Property> tmp = new List<Property>();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                {
                    Property property = new Property(propertyInfo);
                    tmp.Add(property);
                }
            }
            return tmp;
        }

        public List<Constructor> GetConstructors(Type type, BindingFlags bindingFlags)
        {
            ConstructorInfo[] constructors = type.GetConstructors(bindingFlags);
            List<Constructor> tmp = new List<Constructor>();
            foreach (ConstructorInfo constructorInfo in constructors)
            {
                Constructor constructor = new Constructor(constructorInfo);
                tmp.Add(constructor);
            }
            return tmp;
        }
    }
}
