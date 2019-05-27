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

        IAD Add(IAD other);

        IAD Subtract(IAD other);

        IAD Multiply(IAD other);

        IAD Divide(IAD other);

        IAD Divide(double other);

        IAD Negative();

        IAD Exp();

        IAD Log();

        IAD Sqrt();

        IAD Cdf();
    }
}
