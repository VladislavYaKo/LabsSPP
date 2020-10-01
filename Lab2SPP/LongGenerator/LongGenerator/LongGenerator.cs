using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneratorLibrary;

namespace LongGenerator
{
    public class LongGenerator : Generator
    {
        public override object Generate()
        {
            byte[] longArr = new byte[8];
            rnd.NextBytes(longArr);
            return BitConverter.ToInt64(longArr, 0);
        }
        public override Type GeneratorType()
        {
            return typeof(long);
        }
    }
}
