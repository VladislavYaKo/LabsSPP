using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryForTest
{
    public class Class2
    {
        public static List<int> NewList(int a)
        {
            List<int> result = new List<int> { a, a, a };
            return result;
        }

        public static int Count(List<int> list)
        {
            return list.Count;
        }
    }
}
