using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;
using Microsoft.Office.Interop.Excel;

namespace ModelFactory
{
    public class BachelierVanillaCalculator
    {
        public static double Price(double maturity, double strike, double volatility, double tenor)
        {
            var ratesmarket = MarketDataFactory.BuildRatesMarket();

            var nu = Math.Pow(volatility, 2) * maturity;
            if (nu == 0.0) throw new Exception("Divided by zero");
            var xT = (strike - ratesmarket.Instrument(tenor).InitialSwapFunction.Eval(maturity)) / Math.Sqrt(nu);
            return ratesmarket.Instrument(tenor).InitialBPV.Eval(maturity) * Math.Sqrt(nu) * (Normal.PDF(0, 1, xT) - xT * (1 - Normal.CDF(0, 1, xT)));
        }
        public static double PriceAtm(double maturity, double volatility, double tenor)
        {
            var ratesmarket = MarketDataFactory.BuildRatesMarket();

            var nu = Math.Pow(volatility, 2) * maturity;
            var price = ratesmarket.Instrument(tenor).InitialBPV.Eval(maturity) * Math.Sqrt(nu / (2 * Math.PI));
            return price;
        }
        public static double InversePrice(double mcPrice, double maturity, double strike, double tenor)
        {
            Func<double, double> func = x => Price(maturity, strike, x, tenor) - mcPrice;
            double root = Brent.FindRoot(func, 0.0000002, 0.1, 1E-100, 1000000000);
            return root;
        }
    }
}