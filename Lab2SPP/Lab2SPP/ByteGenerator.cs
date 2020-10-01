using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class ByteGenerator : Generator
    {
        public override object Generate()
        {
            return (byte)rnd.Next(byte.MinValue, byte.MaxValue+1);
        }
        public override Type GeneratorType()
        {
            return typeof(byte);
        }
    }
}
