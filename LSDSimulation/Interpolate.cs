using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;

namespace ClassLibrary1
{
    public enum InterpolationType
    {
        CubicSpline,
        Linear
    }
    public enum ExtrapolationType
    {
        Flat,
        Other
    }
    public class InterpolateGrid : RRFunction
    {
        private readonly List<double> xlist;
        private readonly List<double> ylist;
        private readonly InterpolationType interpolationType;
        private readonly double xmin;
        private readonly double xmax;
        private readonly IInterpolation interpolationchoice;
        public InterpolateGrid(List<double> xlist, List<double> ylist, InterpolationType interpolationType)
        {
            this.xlist = xlist;
            this.ylist = ylist;
            this.interpolationType = interpolationType;
            xmin = xlist.Min();
            xmax = xlist.Max();
            switch (interpolationType)
            {
                case InterpolationType.Linear:
                    interpolationchoice = LinearSpline.Interpolate(xlist, ylist);
                    break;
                case InterpolationType.CubicSpline:
                    interpolationchoice = CubicSpline.InterpolateAkima(xlist, ylist);
                    break;
                default:
                    throw new InvalidOperationException("unknown item type");
            }
        }
        public override double Eval(double x)
        {
            if (x >= xmin && x <= xmax) return interpolationchoice.Interpolate(x);    // Interpolation Cubic Splines
            return Eval(x < xmin ? xmin : xmax);   // Flat
        }
        public override bool HasFirstDerivative()
        {
            throw new NotImplementedException();
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }

    public class Interpolation : RRFunction
    {
        private readonly List<double> xlist;
        private readonly List<double> ylist;
        private readonly double xmin;
        private readonly double xmax;
        private readonly CubicSpline interpolation;
        public Interpolation(List<double> xlist, List<double> ylist)
        {
            this.xlist = xlist;
            this.ylist = ylist;
            xmin = xlist.Min();
            xmax = xlist.Max();
            interpolation = CubicSpline.InterpolateAkima(xlist, ylist);
        }
        public override double Eval(double real)
        {
            if (real >= xmin && real <= xmax) return interpolation.Interpolate(real);    // Interpolation Cubic Splines
            return Eval(real < xmin ? xmin : xmax);               // Flat
        }
        public override bool HasFirstDerivative()
        {
            throw new NotImplementedException();
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }
}