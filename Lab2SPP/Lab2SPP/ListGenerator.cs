using GeneratorLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

namespace Lab2SPP
{
    public class ListGenerator
    {
        private Dictionary<Type, Generator> _generators = new Dictionary<Type, Generator>();
        private Faker _faker;
        private Random rnd = new Random();

        public ListGenerator(Dictionary<Type, Generator> generators, Faker faker)
        {
            _generators = generators;
            _faker = faker;
        }
        public object Generate(Type type)
        {
            IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            int listLength = rnd.Next(0, 16);
            for (int i = 0; i < listLength; i++)
                newList.Add(_faker.Create(type));

            return newList;
        }

        public Type GeneratorType()
        {
            throw new NotImplementedException();
        }
    }
}
