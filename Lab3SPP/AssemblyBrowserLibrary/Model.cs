using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AssemblyBrowserLibrary
{
    public class Model
    {
        private List<Namespace> _namespaces;

        public Model()
        {
            _namespaces = new List<Namespace>();
        }

        public List<Namespace> LoadAssembly(string path)
        {
            Assembly asm = Assembly.LoadFrom(path);
            return GetNamespaces(asm.GetTypes());
        }

        private List<Namespace> GetNamespaces(Type[] Assembly)
        {
            foreach (Type type in Assembly)
            {
                Namespace newNamespace = new Namespace(type.Namespace);
                Namespace existingNamespace = _namespaces.FirstOrDefault(namesp => namesp.Name == newNamespace.Name);
                if (existingNamespace == default(Namespace))
                {
                    if (type.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                    {
                        newNamespace.Classes.Add(new Class(type));
                        _namespaces.Add(newNamespace);
                    }
                }
                else
                {
                    if (type.GetCustomAttribute<CompilerGeneratedAttribute>() == null)
                        existingNamespace.Classes.Add(new Class(type));
                }
            }
            return _namespaces;
        }

        public static string GetTypeName(Type type)
        {
            string retType;
            if (type.IsGenericType)
            {
                retType = type.Name;
                retType = retType.Remove(retType.IndexOf('`'));
                retType += '<';
                foreach (Type constructedType in type.GetGenericArguments())
                {
                    retType += GetTypeName(constructedType) + ',';
                }
                retType = retType.Remove(retType.Length - 1);
                retType += '>';
            }
            else
                retType = type.Name;

            return retType;
        }
    }
}
