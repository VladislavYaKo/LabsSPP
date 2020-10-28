using System;
using System.Collections.Generic;
using System.Text;

namespace AssemblyBrowserLibrary
{
    public class Namespace
    {
        public string Name { get; set; }
        public List<Class> Classes { get; set; }

        public Namespace(string name)
        {
            Name = "namespace " + name;
            Classes = new List<Class>();
        }
    }
}
