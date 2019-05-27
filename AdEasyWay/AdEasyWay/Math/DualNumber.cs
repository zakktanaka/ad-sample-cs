using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay.Math
{
    public class DualNumber
    {
        public double Value { get; set; }

        public DualNumber Multiply(DualNumber other)
        {
            return new DualNumber { Value = Value * other.Value };
        }

        public DualNumber Subtract(DualNumber other)
        {
            return new DualNumber { Value = Value - other.Value };
        }
    }
}
