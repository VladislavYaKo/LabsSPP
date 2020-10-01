using System;

namespace Lab2SPP
{
    class Program
    {
        static void Main(string[] args)
        {
            Faker faker = new Faker();

            int a = faker.Create<int>();
            string str = faker.Create<string>();

            Console.WriteLine("{0}\n{1}", a, str);
            Console.ReadKey();
        }
    }
}
