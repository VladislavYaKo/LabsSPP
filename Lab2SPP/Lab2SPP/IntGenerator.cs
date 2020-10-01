using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class IntGenerator : Generator
    {
        public override object Generate()
        {
            return rnd.Next(int.MinValue, int.MaxValue);
        }
        public override Type GeneratorType()
        {
            return typeof(int);
        }
    }
}
