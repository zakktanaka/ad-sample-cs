using MathNet.Numerics.Distributions;
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
            if (other == this)
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

        public IAD Add(IAD other)
        {
            var l = Value;
            var r = other.Value;

            return new TopDownAD
            {
                Value = l + r,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( 1d, (IAD)this),
                    Tuple.Create( 1d, other),
                }
            };
        }

        public IAD Divide(IAD other)
        {
            var l = Value;
            var r = other.Value;

            return new TopDownAD
            {
                Value = l / r,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( 1/r, (IAD)this),
                    Tuple.Create( -l / (r * r), other),
                }
            };
        }

        public IAD Divide(double other)
        {
            var l = Value;
            var r = other;

            return new TopDownAD
            {
                Value = l / r,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( 1/r, (IAD)this),
                }
            };
        }

        public IAD Negative()
        {
            var l = Value;

            return new TopDownAD
            {
                Value = -l,
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create( -1d, (IAD)this),
                }
            };
        }

        public IAD Exp()
        {
            var l = Value;

            return new TopDownAD
            {
                Value = System.Math.Exp(l),
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create(System.Math.Exp(l), (IAD)this),
                }
            };
        }

        public IAD Log()
        {
            var l = Value;

            return new TopDownAD
            {
                Value = System.Math.Log(l),
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create(1/l, (IAD)this),
                }
            };
        }

        public IAD Sqrt()
        {
            var l = Value;

            return new TopDownAD
            {
                Value = System.Math.Sqrt(l),
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create(0.5 / System.Math.Sqrt(l), (IAD)this),
                }
            };
        }

        public IAD Cdf()
        {
            var l = Value;

            return new TopDownAD
            {
                Value = Normal.CDF(0, 1, l),
                polynomial = new List<Tuple<double, IAD>>
                {
                    Tuple.Create(Normal.PDF(0, 1, l), (IAD)this),
                }
            };
        }
    }
}
