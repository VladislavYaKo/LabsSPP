using System;

namespace Lab2SPP
{
    public class ByteGenerator : Generator
    {
        public override object Generate()
        {
            return (byte)rnd.Next(byte.MinValue, byte.MaxValue+1);
        }
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(byte);
        }
    }
}
