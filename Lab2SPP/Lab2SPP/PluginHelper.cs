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

        public static Dictionary<string, Generator> GetGenerators(List<Assembly> Assemblies)
        {
            Dictionary<string, Generator> Generators = new Dictionary<string, Generator>();
            foreach (Assembly assembly in Assemblies)
            {
                try
                {
                    Type[] Types = assembly.GetTypes();
                    PluginHelper.LoadGenerators(Generators, Types);
                }catch(ReflectionTypeLoadException ex)
                {
                    Exception[] exsInfo = ex.LoaderExceptions;
                    foreach (Exception exInfo in exsInfo)
                        Console.WriteLine(exInfo.Message);
                }
                
            }
            return Generators;
        }

        private static void LoadGenerators(Dictionary<string, Generator> Generators, Type[] Types)
        {
            foreach (Type type in Types)
            {
                if (!type.IsInterface && !type.IsAbstract && typeof(Generator).IsAssignableFrom(type))
                {
                    Generator generator = Activator.CreateInstance(type) as Generator;
                    Generators.Add(generator.GeneratorType().Name, generator);                    
                }
            }
        }
    }
}
