 using System;
using System.Threading;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TracerLib;

namespace Lab1SPP
{
    public class Program
    {
        public static Tracer _tracer = new Tracer();
        static string filePath = "Output.txt";
        static void Main(string[] args)
        {            
            _tracer.StartTrace();
            TestMethod3();
            Thread testThread = new Thread(TestMethod);
            testThread.Start();
            Thread.Sleep(200);
            _tracer.StopTrace(); 

            testThread.Join();

            Serializer jsonSer = new JsonSerializer();
            Serializer xmlSer = new XmlSerializer();
            byte[] jsonBytes, xmlBytes;
            jsonBytes = jsonSer.SerializeObj(_tracer.GetTraceResult());
            xmlBytes = xmlSer.SerializeObj(_tracer.GetTraceResult());

            ResultsOutput resOutput = new ResultsOutput();
            resOutput.WriteToConsole(jsonBytes);
            resOutput.WriteToConsole(xmlBytes);
            resOutput.CleanFile(filePath);
            resOutput.WriteToFile(filePath, jsonBytes);
            resOutput.WriteToFile(filePath, xmlBytes);

            Console.ReadKey();
        }

        static void TestMethod()
        {
            _tracer.StartTrace();
            Thread testThread = new Thread(TestMethod3);
            testThread.Start();
            Thread.Sleep(110);
            TestMethod2();
            _tracer.StopTrace();
        }

        static void TestMethod2()
        {
            _tracer.StartTrace();
            TestClass.TestMethod3();
            Thread.Sleep(100);
            TestMethod3();
            _tracer.StopTrace();

        }
        static void TestMethod3()
        {
            _tracer.StartTrace();
            Thread.Sleep(50);
            _tracer.StopTrace();
        }
    }

    public class TestClass
    { 
        public static void TestMethod3()
        {
            Program._tracer.StartTrace();
            Thread.Sleep(50);
            Program._tracer.StopTrace();
        }
    }
    public interface IResultsOutput
    {
        void WriteToConsole(byte[] bytes);
        void WriteToFile(string filePath, byte[] bytes);
        void CleanFile(string filePath);
    }

    public class ResultsOutput : IResultsOutput
    {
        public void WriteToConsole(byte[] bytes)
        {
            string str = Encoding.UTF8.GetString(bytes);
            Console.WriteLine(str);
        }

        public void WriteToFile(string filePath, byte[] bytes)
        {
            string str = Encoding.UTF8.GetString(bytes);
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.Write(str);
            }
        }

        public void CleanFile(string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath, false))
            {
                sw.Write(string.Empty);
            }
        }
    }
}
