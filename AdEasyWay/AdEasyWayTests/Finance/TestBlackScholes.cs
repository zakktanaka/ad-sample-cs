using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdEasyWay.Finance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdEasyWay.Math;

namespace AdEasyWay.Finance.Tests
{
    [TestClass()]
    public class TestBlackScholes
    {
        [TestMethod()]
        public void BlackScholes_EuropeanOptionTest()
        {
            var buStrike = new DualNumber(110);
            var buMaturity = new DualNumber(3);
            var buAtm = new DualNumber(100);
            var buVolatility = new DualNumber(0.2);
            var buRiskFreeRate = new DualNumber(0.05);

            var buBs = BlackScholes.EuropeanOption(
                buStrike,
                buMaturity,
                buAtm,
                buVolatility,
                buRiskFreeRate,
                OptionType.Call
                );


            var tdStrike = new TopDownAD { Value = 110 };
            var tdMaturity = new TopDownAD { Value = 3 };
            var tdAtm = new TopDownAD { Value = 100 };
            var tdVolatility = new TopDownAD { Value = 0.2 };
            var tdRiskFreeRate = new TopDownAD { Value = 0.05 };

            var tdBs = BlackScholes.EuropeanOption(
                tdStrike,
                tdMaturity,
                tdAtm,
                tdVolatility,
                tdRiskFreeRate,
                OptionType.Call
                );

            Assert.AreEqual(buBs.Value, tdBs.Value);
            Assert.AreEqual(buBs.DerivedBy(buStrike), tdBs.DerivedBy(tdStrike));
            Assert.AreEqual(buBs.DerivedBy(buVolatility), tdBs.DerivedBy(tdVolatility));
            Assert.AreEqual(buBs.DerivedBy(buMaturity), tdBs.DerivedBy(tdMaturity));
            Assert.AreEqual(16.210868138551504, buBs.Value, 1e-4);
        }
    }
}