using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                    newNamespace.Classes.Add(new Class(type));
                    _namespaces.Add(newNamespace);
                }
                else
                {
                    existingNamespace.Classes.Add(new Class(type));
                }
            }
            return _namespaces;
        }
    }
}
