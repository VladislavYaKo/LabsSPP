using System;

namespace Lab2SPP
{
    public class ShortGenerator : Generator
    {
        public override object Generate()
        {
            return (short)rnd.Next(short.MinValue, short.MaxValue+1);
        }
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(short);
        }
    }
}
