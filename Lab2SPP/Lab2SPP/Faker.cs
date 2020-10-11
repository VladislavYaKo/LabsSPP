using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;

namespace Lab2SPP
{
    public class Faker
    {
        private Dictionary<string, Generator> _generators = new Dictionary<string, Generator>();
        private Stack<Type> _generationStack = new Stack<Type>();

        public Faker()
        {
            List<Assembly> asms = PluginHelper.LoadPlugins("..\\..\\..\\Plugins\\StringGenerator.dll", "..\\..\\..\\Plugins\\LongGenerator.dll");
            _generators = PluginHelper.GetGenerators(asms);
            _generators.Add(typeof(bool).Name, new BoolGenerator());
            _generators.Add(typeof(byte).Name, new ByteGenerator());
            _generators.Add(typeof(char).Name, new CharGenerator());
            _generators.Add(typeof(DateTime).Name, new DateTimeGenerator());
            _generators.Add(typeof(double).Name, new DoubleGenerator());
            _generators.Add(typeof(float).Name, new FloatGenerator());
            _generators.Add(typeof(int).Name, new IntGenerator());
            _generators.Add(typeof(short).Name, new ShortGenerator());
            _generators.Add(typeof(List<>).Name, new ListGenerator(this));
        }

        public object Create(Type type)
        {
            if (_generationStack.Contains(type))
                return null;

            if (type.IsAbstract || type.IsInterface || type == typeof(void))
                return null;

            Generator curGenerator;
            _generators.TryGetValue(type.Name, out curGenerator);
            if (curGenerator != null)
                return curGenerator.Generate(type);

            
            if (!type.IsPrimitive)
            {
                _generationStack.Push(type);
                ConstructorInfo[] sortedConstructors = GetSortedConstructors(type);
                if (sortedConstructors.Length != 0)
                {
                    foreach (ConstructorInfo ci in sortedConstructors)
                    {
                        var instance  = GenerateObjectFromConstructor(ci);
                        if (instance != null)
                        {
                            instance = GenerateFieldsAndProperties(type, instance);
                            _generationStack.Pop();
                            return instance;
                        }
                    }   
                }
                else
                {
                    if (type.GetConstructors().Count() != 0 || type.IsValueType)
                    {
                        var instance = Activator.CreateInstance(type);
                        instance = GenerateFieldsAndProperties(type, instance);
                        _generationStack.Pop();
                        return instance;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public T Create<T>()
        {
            object value = Create(typeof(T));
            if (value == null)
                return default(T);

            return (T)value;
        }

        private ConstructorInfo[] GetSortedConstructors(Type type)
        {
            ConstructorInfo[] sortedConstructors;
            sortedConstructors = type.GetConstructors();
            return sortedConstructors.OrderByDescending(c => c.GetParameters().Length).ToArray();           
        }

        private object GenerateObjectFromConstructor(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parametersValues = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                Generator valueGenerator;
                _generators.TryGetValue(parameters[i].ParameterType.Name, out valueGenerator);
                parametersValues[i] = valueGenerator.Generate(parameters[i].ParameterType);

            }

            object constructerObj;
            try
            {
                constructerObj = constructor.Invoke(parametersValues);
            }
            catch(Exception)
            {
                return null;
            }
            return constructerObj;            
        }

        private object GetDefaultValue(Type type)
        {
            if (type.IsValueType)
                return Activator.CreateInstance(type);
            else
                return null;
        }

        private object GenerateFieldsAndProperties(Type type, object instance)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                object defVal = GetDefaultValue(field.FieldType);

                bool shouldCreate;
                if (defVal != null)
                    shouldCreate = defVal.Equals(field.GetValue(instance));
                else
                    shouldCreate = (defVal == field.GetValue(instance));

                if (shouldCreate)
                {
                    object value = Create(field.FieldType);
                    field.SetValue(instance, value);
                }
            }
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.CanWrite)
                {
                    object value = Create(property.PropertyType);
                    property.SetValue(instance, value);
                }
            }
            return instance;
        }
    }
}
