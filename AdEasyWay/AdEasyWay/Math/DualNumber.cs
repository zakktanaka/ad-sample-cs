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
            if(differentials.TryGetValue(other, out double ret))
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

        public IAD Subtract(IAD other)
        {
            if(other is DualNumber)
            {
                return Subtract((DualNumber)other); 
            }
            else
            {
                var l = Value;
                var r = other.Value;

                var vals = differentials.Keys;

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

        public IAD Multiply(IAD other)
        {
            if (other is DualNumber)
            {
                return Multiply((DualNumber)other);
            }
            else
            {
                var l = Value;
                var r = other.Value;

                var vals = differentials.Keys;

                var diffs = vals
                    .Select(v => new { Value = v, Differential = r * DerivedBy(v) + l * other.DerivedBy(v) })
                    .ToDictionary(pair => pair.Value, pair => pair.Differential);

                return new DualNumber
                {
                    Value = l - r,
                    differentials = diffs,
                };
            }
        }
    }
}
