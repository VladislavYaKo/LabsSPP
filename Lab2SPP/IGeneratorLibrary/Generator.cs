using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneratorLibrary
{
    public abstract class Generator
    {
        protected Random rnd = new Random();
        public abstract object Generate();
        public abstract Type GeneratorType();
    }
}
