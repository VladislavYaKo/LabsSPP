using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AssemblyBrowserLibrary;
using System.Collections.Generic;

namespace UnitTestsLab3
{
    [TestClass]
    public class UnitTestsLab3
    {
        private Model assemblyBrowser;
        private List<Namespace> namespaces { get; set; }

        [TestInitialize]
        public void Initialize()
        {
            assemblyBrowser = new Model();
            namespaces = assemblyBrowser.LoadAssembly("LibraryForTest.dll");
        }

        [TestMethod]
        public void DLLLoad_ShouldLoadDLL()
        {
            Assert.IsNotNull(namespaces);
        }

        [TestMethod]
        public void ShouldReturnOneNamespace()
        {
            Assert.AreEqual(1, namespaces.Count);
        }

        public void ShouldReturnTwoClasses()
        {
            Assert.AreEqual(2, namespaces[0].Classes.Count);
        }

        [TestMethod]
        public void ShouldReturnpublicClassClass1()
        {
            Assert.AreEqual("public class Class1", namespaces[0].Classes[0].Name);
        }

        [TestMethod]
        public void ShouldReturnTwoConstructors()
        {
            Assert.AreEqual(2, namespaces[0].Classes[0].Constructors.Count);
        }

        [TestMethod]
        public void ShouldReturnFourFields()
        {
            Assert.AreEqual(4, namespaces[0].Classes[0].Fields.Count);
        }

        [TestMethod]
        public void ShouldReturnOneProperty()
        {
            Assert.AreEqual(1, namespaces[0].Classes[0].Properties.Count);
        }

        [TestMethod]
        public void ShouldReturnOneMethod()
        {
            Assert.AreEqual(1, namespaces[0].Classes[0].Methods.Count);
        }
    }
}
