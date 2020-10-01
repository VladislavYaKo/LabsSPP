﻿using GeneratorLibrary;
using System;
using System.Collections.Generic;
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
                //TODO
            }
            return null;
        }

        public T Create<T>()
        {
            if (Create(typeof(T)) == null)
                return default(T);

            return (T)Create(typeof(T));
        }
    }
}
