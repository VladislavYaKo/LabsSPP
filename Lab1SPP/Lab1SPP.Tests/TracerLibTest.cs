using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TracerLib;

namespace Lab1SPP.Tests
{
    public class TestTracer
    {
        public static Tracer tracer = new Tracer();
    }

    public class TestClass
    {
        public static void TestMethod3()
        {
            TestTracer.tracer.StartTrace();
            Thread.Sleep(50);
            TestTracer.tracer.StopTrace();
        }
    }
    public class TestMain
    {
        public static void Main()
        {
            TestTracer.tracer.StartTrace();
            TestMethod3();
            Thread testThread = new Thread(TestMethod);
            testThread.Start();
            Thread.Sleep(200);
            TestMethod4();
            TestTracer.tracer.StopTrace();
            testThread.Join();
        }

        static void TestMethod()
        {
            TestTracer.tracer.StartTrace();
            Thread testThread = new Thread(TestMethod3);
            testThread.Start();
            Thread.Sleep(110);
            TestMethod2();
            TestTracer.tracer.StopTrace();
        }

        static void TestMethod2()
        {
            TestTracer.tracer.StartTrace();
            TestClass.TestMethod3();
            Thread.Sleep(100);
            TestTracer.tracer.StopTrace();

        }
        static void TestMethod3()
        {
            TestTracer.tracer.StartTrace();
            Thread.Sleep(50);
            TestTracer.tracer.StopTrace();
        }

        static void TestMethod4()
        {
            Thread.Sleep(10);
            TestMethod5();
        }

        static void TestMethod5()
        {
            TestTracer.tracer.StartTrace();
            Thread.Sleep(40);
            TestTracer.tracer.StopTrace();
        }
    }
    [TestClass]
    public class TracerLibTest
    {
        [TestMethod]
        public void AGetResult()
        {
            TestMain.Main();
        }

        [TestMethod]
        public void TestThreadCount()
        {
            List<ThreadTraceResult> result = TestTracer.tracer.GetTraceResult();

            Assert.AreEqual((int)3, result.Count);
        }

        [TestMethod]
        public void TestThreadExecTime()
        {
            List<ThreadTraceResult> result = TestTracer.tracer.GetTraceResult();

            Assert.IsTrue((long)300 <= result[0].execTime);
            Assert.IsTrue((long)260 <= result[1].execTime);
            Assert.IsTrue((long)50 <= result[2].execTime);
            //Assert.IsNotNull(result[2]);
        }

        [TestMethod]
        public void TestMethodName()
        {
            List<ThreadTraceResult> result = TestTracer.tracer.GetTraceResult();

            Assert.AreEqual((string)"Main", result[0].calledMethods[0].methodName);
            Assert.AreEqual((string)"TestMethod", result[1].calledMethods[0].methodName);
        }

        [TestMethod]
        public void TestClassName()
        {
            List<ThreadTraceResult> result = TestTracer.tracer.GetTraceResult();

            Assert.AreEqual((string)"TestMain", result[0].calledMethods[0].className);
            Assert.AreEqual((string)"TestClass", result[1].calledMethods[0].calledMethods[0].calledMethods[0].className);
        }

        [TestMethod]
        public void TestOverjump()
        {
            List<ThreadTraceResult> result = TestTracer.tracer.GetTraceResult();

            Assert.AreEqual("TestMethod5", result[0].calledMethods[0].calledMethods[1].methodName);
        }
    }
}
