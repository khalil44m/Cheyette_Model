using System;
using System.Collections.Generic;
using ClassLibrary1;
using LSDSimulation;
using MathNet.Numerics.RootFinding;

namespace Calibration
{
    public interface ICalibrationGrid
    {
        List<double> GridEval(double t);
    }

    public class LocalCalibrationGrid : ICalibrationGrid
    {
        private readonly MarketData marketdata;
        private readonly double tenor;
        public LocalCalibrationGrid(MarketData marketdata, double tenor)
        {
            this.marketdata = marketdata;
            this.tenor = tenor;
        }
        public List<double> GridEval(double t)
        {
            double alpha = 0.001;
            double coupon = marketdata.RatesMarket.Instrument(tenor).InitialBPV.Eval(t);
            Func<double, double> funcmax = x => marketdata.ImpliedVolatilityMarket.Skew.Eval(t, x) + alpha * coupon;
            Func<double, double> funcmin = x => marketdata.ImpliedVolatilityMarket.Skew.Eval(t, x) + (1 - alpha) * coupon;
            double max = Brent.FindRoot(funcmax, 0.0, 0.2, 0.00000001, 1000);
            double min = Brent.FindRoot(funcmin, -0.1, 0.1, 0.00000001, 1000);
            var sizegrid = (int)Math.Round(Math.Max(30 * Math.Sqrt(t), 15));
            double step = (max - min) / (sizegrid - 1);
            var grid = TimeGrid.RegularGrid(min, max, step);
            grid.RemoveAt(0);
            grid.RemoveAt(grid.Count - 1);
            grid.RemoveAt(grid.Count - 1);
            return grid;
        }
    }
}