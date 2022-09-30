using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using LSDSimulation;
using MathNet.Numerics.Interpolation;

// ReSharper disable once CheckNamespace
namespace LSDSimulation
{
    public class MarketData
    {
        private readonly RatesMarket ratesMarket;
        private readonly ImpliedVolatilityMarket impliedVolatilityMarket;
        private readonly AtmImpliedVolatilityMarket atmImpliedVolatilityMarket;

        public MarketData(RatesMarket ratesMarket, ImpliedVolatilityMarket impliedVolatilityMarket, AtmImpliedVolatilityMarket atmImpliedVolatilityMarket)
        {
            this.ratesMarket = ratesMarket;
            this.impliedVolatilityMarket = impliedVolatilityMarket;
            this.atmImpliedVolatilityMarket = atmImpliedVolatilityMarket;
        }
        public RatesMarket RatesMarket => ratesMarket;
        public ImpliedVolatilityMarket ImpliedVolatilityMarket => impliedVolatilityMarket;
        public AtmImpliedVolatilityMarket AtmImpliedVolatilityMarket => atmImpliedVolatilityMarket;
    }
    public class RatesMarket
    {
        private readonly double[] zcData;
        private readonly double[] dates;
        private readonly RRFunction z0t;
        private readonly RRFunction forwardfunction;
        public RatesMarket(double[] zcData, double[] dates, RRFunction z0t, RRFunction forwardfunction)
        {
            this.zcData = zcData;
            this.dates = dates;
            this.z0t = z0t;
            this.forwardfunction = forwardfunction;
        }

        public InstrMarket Instrument(double tenor)
        {
            Func<double, double> bpvfunction = x =>
            {
                var scheduledates = TimeGrid.RegularGrid(x + 1, x + tenor, 1);
                double result = 0;
                foreach (var y in scheduledates) result += z0t.Eval(y);
                return result;
            };
            var bpv = new ConverttoRRFunction(bpvfunction);

            Func<double, double> swapfunction = x => (z0t.Eval(x) - z0t.Eval(x + tenor)) / bpv.Eval(x);
            var swapratefunction = new ConverttoRRFunction(swapfunction);

            return new InstrMarket(bpv, swapratefunction);
        }
        public double[] ZcData => zcData;
        public double[] Dates => dates;
        public RRFunction InitialZeroCoupon => z0t;
        public RRFunction InitialForwardRate => forwardfunction;
    }

    public class InstrMarket
    {
        private readonly RRFunction bpv;
        private readonly RRFunction swapFunction;

        public InstrMarket(RRFunction bpv, RRFunction swapFunction)
        {
            this.bpv = bpv;
            this.swapFunction = swapFunction;
        }
        public RRFunction InitialBPV => bpv;
        public RRFunction InitialSwapFunction => swapFunction;
    }

    public class ImpliedVolatilityMarket
    {
        private readonly double tenor;
        private readonly R2RFunction continuousIvm;
        private readonly R2RFunction derivativetime; // The first derivative of the Market Price (time)
        private readonly R2RFunction skew;   // The first derivative of the Market Price (strike)
        private readonly R2RFunction convexity; // The second derivative of the Market Price
        private readonly R2RFunction sigmalocfunctionmarket; // The market formula in Local Calibration
        private readonly R2RFunction sigmaloctildefunctionmarket; // The market formula in Global Calibration
        public ImpliedVolatilityMarket(double tenor, R2RFunction continuousIvm, R2RFunction derivativetime, R2RFunction skew, R2RFunction convexity, R2RFunction sigmalocfunctionmarket, R2RFunction sigmaloctildefunctionmarket)
        {
            this.tenor = tenor;
            this.continuousIvm = continuousIvm;
            this.derivativetime = derivativetime;
            this.skew = skew;
            this.convexity = convexity;
            this.sigmalocfunctionmarket = sigmalocfunctionmarket;
            this.sigmaloctildefunctionmarket = sigmaloctildefunctionmarket;
        }
        public R2RFunction ContinuousIVM => continuousIvm;
        public R2RFunction DerivativeTime => derivativetime;
        public R2RFunction Skew => skew;
        public R2RFunction SmileConvexity => convexity;
        public R2RFunction SigmaLocMarket => sigmalocfunctionmarket;
        public R2RFunction SigmaLocTildeMarket => sigmaloctildefunctionmarket;

    }

    public class AtmImpliedVolatilityMarket
    {
        private readonly RRFunction derivativetime; // The first derivative of the Market Price (time)
        public AtmImpliedVolatilityMarket(RRFunction derivativetime)
        {
            this.derivativetime = derivativetime;
        }
        public RRFunction DerivativeTime => derivativetime;
    }
}
