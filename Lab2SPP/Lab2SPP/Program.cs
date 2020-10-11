using System;
using System.Collections.Generic;

namespace Lab2SPP
{
    public class A
    {
        int c;
        bool k;
        List<byte> T;
        public int testint { get; set; }
    }

    public class B
    {
        public C c { get; set; }
    }

    public class C
    {
        public B b { get; set; }
    }

    public class D
    {
        public int a;
        public int b;
        public bool c;
        private List<bool> BoolList;
        public char k;
        public string Name { get; set; }

        public D()
        {
            this.a = 10;
            this.b = 2;
        }
        public D(int a, int b, bool c, List<bool> listbool)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.BoolList = listbool;
        }
        public D(int k, int e)
        {
            this.a = e;
            this.b = k;
        }
    }

    public class Foo
    {
        int k;
        public InsideTheFoo inside { get; set; }
        public Foo(int a)
        {
            this.k = a;
        }
    }

    public class InsideTheFoo
    {
        int c;
        public Foo foo { get; set; }
        public InsideTheFoo(int k)
        {
            this.c = k;
        }
    }

    public class PrivateConstructor
    {
        int k;
        int e;
        private PrivateConstructor(int a, int b)
        {
            k = a;
            e = b;
        }
    }
    public class TestExceptionConstruct
    {
        public int a;
        public char b;
        public string str;

        public TestExceptionConstruct()
        {
            b = 'f';
            str = "Hello";
        }

        public TestExceptionConstruct(int aa, char bb)
        {
            throw new NotImplementedException();
            a = aa;
            b = bb;            
        }
    }
    

    public class PrivateProperty
    {
        private string Name { get; }
    }

    public class TestClass
    {
        public int a;
        bool b;
    }
    public struct TestStruct
    {
        public int a;
        public string str;

    }

    public struct TestStructConstructor
    {
        public int a;
        public string str;

        public TestStructConstructor(int a, string str)
        {
            this.a = a;
            this.str = str;
        }
    }
    public class Program
    {
        
        static void Main(string[] args)
        {
            Faker faker = new Faker();

            int a = faker.Create<int>();
            string str = faker.Create<string>();
            DateTime dt = faker.Create<DateTime>();
            List<int> testList = faker.Create<List<int>>();
            Console.WriteLine("{0}\n{1}\n{2}\n", a, str, dt);
            foreach (int i in testList)
                Console.Write(i + " ");

            Console.WriteLine();
            TestStruct ts = faker.Create<TestStruct>();
            TestStructConstructor tsc = faker.Create<TestStructConstructor>();
            Console.WriteLine("{0}, {1}\n{2}, {3}", ts.a, ts.str, tsc.a, tsc.str);

            Console.WriteLine();
            List<List<int>> listOfList = faker.Create<List<List<int>>>();
            foreach(List<int> listElem in listOfList)
            {
                foreach (int intElem in listElem)
                    Console.Write(intElem + " ");
                Console.WriteLine();
            }

            List<TestClass> l = faker.Create<List<TestClass>>();

            Console.ReadKey();
        }
    }
}
