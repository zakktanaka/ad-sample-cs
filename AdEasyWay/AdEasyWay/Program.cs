using AdEasyWay.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay
{
    class Program
    {
        private static DualNumber Function(DualNumber x, DualNumber y)
        {
            return x.Multiply(x).Subtract(y.Multiply(y));
        }

        private static void TestFunction()
        {
            var x = new DualNumber { Value = 1 };
            var y = new DualNumber { Value = 2 };

            var ans = Function(x, y);

            Console.WriteLine(ans.Value == (1 * 1 - 2 * 2));
        }

        static void Main(string[] args)
        {
            TestFunction();
            Console.ReadKey();
        }
    }
}
