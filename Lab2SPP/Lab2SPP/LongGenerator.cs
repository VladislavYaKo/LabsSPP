using GeneratorLibrary;
using System;

namespace Lab2SPP
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
