using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class DoubleGenerator : Generator
    {
        public override object Generate(Type reqType)
        {
            return rnd.NextDouble();
        }
        public override Type GeneratorType()
        {
            return typeof(double);
        }
    }
}
