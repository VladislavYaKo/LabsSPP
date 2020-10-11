using System;

namespace Lab2SPP
{
    public class DateTimeGenerator : Generator
    {
        public override object Generate()
        {
            DateTime curDateTime = new DateTime(rnd.Next(1, 9999), rnd.Next(1, 12), rnd.Next(1, 28), 
                rnd.Next(0, 23), rnd.Next(0, 59), rnd.Next(0, 59));
            return curDateTime;
        }
        public override object Generate(Type reqType)
        {
            return Generate();
        }
        public override Type GeneratorType()
        {
            return typeof(DateTime);
        }
    }
}
