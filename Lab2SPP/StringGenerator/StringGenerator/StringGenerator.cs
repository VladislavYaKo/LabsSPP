using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lab2SPP;

namespace StringGenerator
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
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(string);
        }
    }
}
