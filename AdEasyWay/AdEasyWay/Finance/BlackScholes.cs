using AdEasyWay.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdEasyWay.Finance
{
    public static class BlackScholes
    {
        private static IAD D1(
            IAD strike,
            IAD maturity,
            IAD atm,
            IAD volatility,
            IAD riskfreerate
            )
        {
            return atm.Divide(strike).Log()
                .Add(
                riskfreerate.Add(volatility.Multiply(volatility).Divide(2).Multiply(maturity))
                )
                .Divide(
                volatility.Multiply(maturity.Sqrt())
                );
            //return (Math.Log(atm / strike) + (riskfreerate + volatility * volatility / 2) * maturity) / (volatility * Math.Sqrt(maturity));
        }

        public static IAD EuropeanOption(
            IAD strike,
            IAD maturity,
            IAD atm,
            IAD volatility,
            IAD riskfreerate,
            OptionType optionType)
        {
            var st = volatility.Multiply(maturity.Sqrt());
            // var st = volatility * Math.Sqrt(maturity);
            var d1 = D1(strike, maturity, atm, volatility, riskfreerate);
            var d2 = d1.Subtract(st);

            return optionType == OptionType.Call ?
                atm.Multiply(d1.Cdf()).Subtract(strike.Multiply(riskfreerate.Negative().Multiply(maturity)).Exp().Multiply(d2.Cdf())) :
                atm.Negative().Multiply(d1.Negative().Cdf()).Subtract(strike.Multiply(riskfreerate.Negative().Multiply(maturity)).Exp().Multiply(d2.Negative().Cdf()));
                //return optionType == OptionType.Call ?
            //     atm * Normal.Cdf(d1) - strike * Math.Exp(-riskfreerate * maturity) * Normal.Cdf(d2) :
            //     -atm * Normal.Cdf(-d1) + strike * Math.Exp(-riskfreerate * maturity) * Normal.Cdf(-d2);
        }
    }
}
