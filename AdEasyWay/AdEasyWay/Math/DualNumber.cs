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
                .ToDictionary(pair =>pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l * r,
                differentials = diffs,
            };
        }
    }
}
