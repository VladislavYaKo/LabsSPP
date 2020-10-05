using GeneratorLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Lab2SPP
{
    public class Faker
    {
        private Dictionary<Type, Generator> _generators = new Dictionary<Type, Generator>();
        private Stack<Type> _generationStack = new Stack<Type>();
        private ListGenerator _listGenerator;

        public Faker()
        {
            List<Assembly> asms = PluginHelper.LoadPlugins("..\\..\\..\\Plugins\\StringGenerator.dll", "..\\..\\..\\Plugins\\LongGenerator.dll");
            _generators = PluginHelper.GetGenerators(asms);
            _generators.Add(typeof(bool), new BoolGenerator());
            _generators.Add(typeof(byte), new ByteGenerator());
            _generators.Add(typeof(char), new CharGenerator());
            _generators.Add(typeof(DateTime), new DateTimeGenerator());
            _generators.Add(typeof(double), new DoubleGenerator());
            _generators.Add(typeof(float), new FloatGenerator());
            _generators.Add(typeof(int), new IntGenerator());
            _generators.Add(typeof(short), new ShortGenerator());

            _listGenerator = new ListGenerator(_generators, this);
        }

        public object Create(Type type)
        {
            if (_generationStack.Contains(type))
                return null;

            if (type.IsAbstract || type.IsInterface || type == typeof(void))
                return null;

            Generator curGenerator;
            _generators.TryGetValue(type, out curGenerator);
            if (curGenerator != null)
                return curGenerator.Generate();
            if (type.IsGenericType)
            {
                return _listGenerator.Generate((Type)type.GetGenericArguments().GetValue(0));
            }
            
            if (!type.IsPrimitive)
            {
                _generationStack.Push(type);
                ConstructorInfo ConstructorWithMaxArgs = GetConstructorWithMaxParams(type);
                if (ConstructorWithMaxArgs != null)
                {
                    var instance = GenerateObjectFromConstructor(ConstructorWithMaxArgs);
                    instance = GenerateFieldsAndProperties(type, instance);
                    _generationStack.Pop();
                    return instance;
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

        private ConstructorInfo GetConstructorWithMaxParams(Type type)
        {
            ConstructorInfo constructorwithmaxparams = null;
            int count = 0;
            foreach (ConstructorInfo constructor in type.GetConstructors())
            {
                if (count < constructor.GetParameters().Count())
                {
                    constructorwithmaxparams = constructor;
                    count = constructor.GetParameters().Count();
                }
            }
            return constructorwithmaxparams;
        }

        private object GenerateObjectFromConstructor(ConstructorInfo constructor)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parametersValues = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsGenericType)
                {
                    parametersValues[i] = _listGenerator.Generate((Type)parameters[i].ParameterType.GenericTypeArguments.GetValue(0));
                }
                else
                {
                    Generator valueGenerator;
                    _generators.TryGetValue(parameters[i].ParameterType, out valueGenerator);
                    parametersValues[i] = valueGenerator.Generate();
                }
            }
            return constructor.Invoke(parametersValues);
        }

        private object GenerateFieldsAndProperties(Type type, object instance)
        {
            FieldInfo[] fields = type.GetFields();
            foreach (FieldInfo field in fields)
            {
                object value = Create(field.FieldType);
                field.SetValue(instance, value);
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
