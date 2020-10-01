using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class BoolGenerator : Generator
    {
        public override object Generate()
        {
            byte curBool =  (byte)rnd.Next(0,2);
            return curBool == 0 ? false : true;
        }
        public override Type GeneratorType()
        {
            return typeof(bool);
        }
    }
}
