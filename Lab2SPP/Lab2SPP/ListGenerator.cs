using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;

namespace Lab2SPP
{
    public class ListGenerator : Generator
    {

        public ListGenerator(Faker faker)
        {
            this.faker = faker;
        }


        public override object Generate(Type type)
        {
            Type listType = type.GenericTypeArguments[0];
            IList newList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(listType));
            int listLength = rnd.Next(0, 16);
            for (int i = 0; i < listLength; i++)
                newList.Add(faker.Create(listType));

            return newList;
        }
        public override object Generate()
        {
            throw new NotImplementedException();
        }

        public override Type GeneratorType()
        {
            throw new NotImplementedException();
        }
    }
}
