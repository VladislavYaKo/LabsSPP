using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class CharGenerator : Generator
    {
        public override object Generate()
        {
            return (char)rnd.Next(char.MinValue, char.MaxValue+1);
        }
        public override Type GeneratorType()
        {
            return typeof(char);
        }
    }
}
