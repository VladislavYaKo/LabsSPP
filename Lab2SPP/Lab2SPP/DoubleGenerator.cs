using System;

namespace Lab2SPP
{
    public class DoubleGenerator : Generator
    {
        public override object Generate()
        {
            return rnd.NextDouble();
        }
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(double);
        }
    }
}
