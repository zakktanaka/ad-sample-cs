using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay.Math
{
    public interface IAD
    {
        double Value { get; }

        double DerivedBy(IAD other);

        IAD Subtract(IAD other);

        IAD Multiply(IAD other);
    }
}
