using System;
using System.Collections.Generic;
using Lab2SPP;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Lab2SPP.Tests
{
    [TestClass]
    public class Lab2SPPTests
    {
        private Faker _faker = new Faker();
        [TestMethod]
        public void TestFoo()
        {
            Foo foo = _faker.Create<Foo>();
            Assert.AreNotEqual(foo, default(Foo));
            Assert.AreNotEqual(foo.inside, default(InsideTheFoo));            
        }
        [TestMethod]
        public void TestEnum()
        {
            Enum e = _faker.Create<Enum>();
            Assert.AreEqual(e, default(Enum));
        }
        [TestMethod]
        public void TestA()
        {
            A a = _faker.Create<A>();
            Assert.AreNotEqual(a, default(A));
        }
        [TestMethod]
        public void TestB()
        {
            B b = _faker.Create<B>();
            Assert.AreNotEqual(b, default(B));
        }
        [TestMethod]
        public void TestC()
        {
            C c = _faker.Create<C>();
            Assert.AreNotEqual(c, default(C));
        }
        [TestMethod]
        public void TestD()
        {
            D d = _faker.Create<D>();
            Assert.AreNotEqual(d, default(D));
            Assert.AreNotEqual(d.a, default(int));
            Assert.AreNotEqual(d.b, default(int));
            Assert.AreNotEqual(d.k, default(char));
        }
        [TestMethod]
        public void TestPrivateConstructor()
        {
            PrivateConstructor pc = _faker.Create<PrivateConstructor>();
            Assert.AreEqual(pc, default(PrivateConstructor));
        }
        [TestMethod]
        public void TestPrivateProperty()
        {
            PrivateProperty pp = _faker.Create<PrivateProperty>();
            Assert.AreNotEqual(pp, default(PrivateProperty));
        }
        [TestMethod]
        public void TestListClass()
        {
            List<TestClass> l = _faker.Create<List<TestClass>>();
            Assert.AreNotEqual(l, default(List<TestClass>));
        }
        [TestMethod]
        public void TestDefaultStruct()
        {
            TestStruct ts = _faker.Create<TestStruct>();
            Assert.AreNotEqual(ts, default(TestStruct));
            Assert.AreNotEqual(ts.a, default(int));
            Assert.AreNotEqual(ts.str, default(string));
        }
        [TestMethod]
        public void TestStructConstructor()
        {
            TestStructConstructor tsc = _faker.Create<TestStructConstructor>();
            Assert.AreNotEqual(tsc, default(TestStructConstructor));
        }
        [TestMethod]
        public void TestListOfList()
        {
            List<List<int>> listOfList = _faker.Create<List<List<int>>>();
            bool isWorking = true;
            int defInts = 0;
            foreach (List<int> listElem in listOfList)
            {
                if (listElem == null)
                {
                    isWorking = false;
                    break;
                }
                else
                {
                    foreach (int intElem in listElem)
                    {
                        if (intElem == default(int))
                            defInts++;
                    }
                    if (defInts == listElem.Count)
                    {
                        isWorking = false;
                        break;
                    }
                }
            }

            Assert.AreEqual(isWorking, true);
        }
        [TestMethod]
        public void TestExceptionConstruct()
        {
            TestExceptionConstruct tex = _faker.Create<TestExceptionConstruct>();
            Assert.AreEqual(tex.b, 'f');
            Assert.AreEqual(tex.str, "Hello");
        }
    }
}
