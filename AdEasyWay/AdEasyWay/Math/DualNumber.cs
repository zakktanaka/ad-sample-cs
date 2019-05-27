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

        private Dictionary<DualNumber, double> differentials;

        private DualNumber()
        {
        }

        public DualNumber(double value)
        {
            Value = value;
            differentials = new Dictionary<DualNumber, double>();
            differentials.Add(this, 1);
        }

        public double DerivedBy(DualNumber other)
        {
            if(differentials.TryGetValue(other, out double ret))
            {
                return ret;
            }
            else
            {
                return 0;
            }
        }

        public DualNumber Multiply(DualNumber other)
        {
            var l = Value;
            var r = other.Value;

            var vals = differentials.Keys
                .Concat(other.differentials.Keys)
                .Distinct();

            var diffs = vals
                .Select(v => new { Value = v, Differential = r * DerivedBy(v) + l * other.DerivedBy(v) })
                .ToDictionary(pair =>pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l * r,
                differentials = diffs,
            };
        }

        public DualNumber Subtract(DualNumber other)
        {
            var l = Value;
            var r = other.Value;

            var vals = differentials.Keys
                .Concat(other.differentials.Keys)
                .Distinct();

            var diffs = vals
                .Select(v => new { Value = v, Differential = DerivedBy(v) - other.DerivedBy(v) })
                .ToDictionary(pair => pair.Value, pair => pair.Differential);

            return new DualNumber
            {
                Value = l - r,
                differentials = diffs,
            };
        }
    }
}
