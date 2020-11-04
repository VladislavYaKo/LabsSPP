using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForTest
{
    public class Class1
    {
        public int a;
        public bool b { get; private set; }
        private string _Class1String;
        protected string ProtectedString = "I am protected";
        internal List<string> InternalList;

        public void Create(int a)
        {
            this.a = a;
            b = true;
        }
        public Class1(int a, bool b)
        {
            this.a = a;
            this.b = b;
        }
        private Class1(string s)
        {
            _Class1String = s;
        }
    }
}
