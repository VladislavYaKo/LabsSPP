using System;

namespace Lab2SPP
{
    public class FloatGenerator : Generator
    {
        public override object Generate()
        {
            return (float)rnd.NextDouble();
        }
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(float);
        }
    }
}
