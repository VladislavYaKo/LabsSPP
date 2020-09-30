using GeneratorLibrary;
using System;

namespace Lab2SPP
{
    public class StringGenerator : Generator
    {
        public override object Generate()
        {
            int strLength = rnd.Next(1, 25);
            string res = "";
            for (int i = 0; i < strLength; i++)
            {
                res += (char)rnd.Next(char.MinValue, char.MaxValue + 1);
            }
            return res;
        }
        public override Type GeneratorType()
        {
            return typeof(string);
        }
    }
}
