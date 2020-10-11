using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2SPP
{
    public abstract class Generator
    {
        //protected GeneratorContext context;
        protected Random rnd = new Random();
        public Faker faker;
        public abstract object Generate();
        public abstract object Generate(Type reqType);
        public abstract Type GeneratorType();
        
        /*public object Generate(Type type)
        {
            IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(type));
            int listLength = rnd.Next(0, 16);
            for (int i = 0; i < listLength; i++)
                newList.Add(faker.Create(type));

            return newList;
        }*/
    }

    /*public class GeneratorContext
    {
        public Random rnd;
        public Type generatorType { get; }
        public Faker faker;

        public GeneratorContext(Random random, Type reqType, Faker faker)
        {
            this.rnd = random;
            this.generatorType = reqType;
            this.faker = faker;
        }

    }*/
}
