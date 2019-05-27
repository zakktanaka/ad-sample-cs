using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay.Math
{
    public class TopDownAD : IAD
    {
        private List<Tuple<double, IAD>> polynomial = new List<Tuple<double, IAD>>();

        public double Value { get; set; }

        public double DerivedBy(IAD other)
        {
            if(other == this)
            {
                return 1;
            }
            else
            {
                return polynomial
                    .Sum(term => term.Item1 * term.Item2.DerivedBy(other));
            }
        }

        public IAD Subtract(IAD other)
        {
            var l = Value;
            var r = other.Value;

            return new TopDownAD
            {
                Value = l - r,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( 1d, (IAD)this),
                    Tuple.Create( -1d, other),
                }

            };
        }

        public IAD Multiply(IAD other)
        {
            var l = Value;
            var r = other.Value;

            return new TopDownAD
            {
                Value = l * r,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( r, (IAD)this),
                    Tuple.Create( l, other),
                }

            };
        }

    }
}
