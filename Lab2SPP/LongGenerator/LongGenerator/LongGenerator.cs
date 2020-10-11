using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2SPP;

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
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(long);
        }
    }
}
