using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay.Math
{
    public class DualNumber : IAD
    {
        public double Value { get; set; }

        private Dictionary<IAD, double> differentials;

        private DualNumber()
        {
        }

        public DualNumber(double value)
        {
            Value = value;
            differentials = new Dictionary<IAD, double>();
            differentials.Add(this, 1);
        }

        public double DerivedBy(IAD other)
        {
            if (differentials.TryGetValue(other, out double ret))
            {
                return ret;
            }
            else
            {
                return 0;
            }
        }

        public DualNumber Subtract(DualNumber other)
        {
            return Subtract(other, GetKeysWith(other));
        }

        public DualNumber Multiply(DualNumber other)
        {
            return Multiply(other, GetKeysWith(other));
        }

        public IAD Subtract(IAD other)
        {
            return Subtract(other, GetKeysWith(other));
        }

        public IAD Multiply(IAD other)
        {
            return Multiply(other, GetKeysWith(other));
        }

        private IEnumerable<IAD> GetKeysWith(IAD other)
        {
            if (other is IAD)
            {
                return GetKeysWith((DualNumber)other);
            }
            else
            {
                return differentials.Keys;
            }
        }

        private IEnumerable<IAD> GetKeysWith(DualNumber other)
        {
            return differentials.Keys
                .Concat(other.differentials.Keys)
                .Distinct();
        }

        private DualNumber Subtract(IAD other, IEnumerable<IAD> vals)
        {
            var l = Value;
            var r = other.Value;

            var diffs = vals
                .Select(v => new { Value = v, Differential = DerivedBy(v) - other.DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l - r,
                differentials = diffs,
            };
        }

        private DualNumber Multiply(IAD other, IEnumerable<IAD> vals)
        {
            var l = Value;
            var r = other.Value;

            var diffs = vals
                .Select(v => new { Value = v, Differential = r * DerivedBy(v) + l * other.DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l * r,
                differentials = diffs,
            };
        }

        public IAD Add(IAD other)
        {
            return Add(other, GetKeysWith(other));
        }

        private DualNumber Add(IAD other, IEnumerable<IAD> vals)
        {
            var l = Value;
            var r = other.Value;

            var diffs = vals
                .Select(v => new { Value = v, Differential = DerivedBy(v) + other.DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l + r,
                differentials = diffs,
            };
        }

        public IAD Divide(IAD other)
        {
            return Divide(other, GetKeysWith(other));
        }

        private DualNumber Divide(IAD other, IEnumerable<IAD> vals)
        {
            var l = Value;
            var r = other.Value;

            var diffs = vals
                .Select(v => new { Value = v, Differential = 1 / r * DerivedBy(v) - l / (r * r) * other.DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l / r,
                differentials = diffs,
            };
        }

        public IAD Divide(double other)
        {
            var l = Value;
            var r = other;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = 1 / r * DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l / r,
                differentials = diffs,
            };
        }

        public IAD Negative()
        {
            var l = Value;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = -DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = -l,
                differentials = diffs,
            };
        }

        public IAD Exp()
        {
            var l = Value;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = System.Math.Exp(l) * DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = System.Math.Exp(l),
                differentials = diffs,
            };
        }

        public IAD Log()
        {
            var l = Value;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = 1 / l * DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = System.Math.Log(Value),
                differentials = diffs,
            };
        }

        public IAD Sqrt()
        {
            var l = Value;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = 0.5 / System.Math.Sqrt(l) * DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = System.Math.Sqrt(l),
                differentials = diffs,
            };
        }

        public IAD Cdf()
        {
            var l = Value;

            var diffs = differentials.Keys
                .Select(v => new { Value = v, Differential = Normal.PDF(0, 1, l) * DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = Normal.CDF(0, 1, l),
                differentials = diffs,
            };
        }
    }
}
