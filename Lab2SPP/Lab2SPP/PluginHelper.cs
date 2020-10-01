using GeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lab2SPP
{
    public class PluginHelper
    {
        public static List<Assembly> LoadPlugins(params string[] paths)
        {
            List<Assembly> asmsList = new List<Assembly>();
            foreach (string path in paths)
            {
                asmsList.Add(Assembly.LoadFrom(path));
            }

            return asmsList;
        }

        public static Dictionary<Type, Generator> GetGenerators(List<Assembly> Assemblies)
        {
            Dictionary<Type, Generator> Generators = new Dictionary<Type, Generator>();
            foreach (Assembly Assembly in Assemblies)
            {
                Type[] Types = Assembly.GetTypes();
                PluginHelper.LoadGenerators(Generators, Types);
                
            }
            return Generators;
        }

        private static void LoadGenerators(Dictionary<Type, Generator> Generators, Type[] Types)
        {
            foreach (Type type in Types)
            {
                if (!type.IsInterface && !type.IsAbstract && typeof(Generator).IsAssignableFrom(type))
                {
                    Generator generator = Activator.CreateInstance(type) as Generator;
                    Generators.Add(generator.GeneratorType(), generator);                    
                }
            }
        }
    }
}
