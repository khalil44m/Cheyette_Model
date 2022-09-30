using System;
using System.Collections.Generic;
using System.Text;
using LSDSimulation;

namespace Calibration
{
    public abstract class IRegulazingKernel
    {
        protected const double K = 0.08;
        protected const double Tmin = 0.25;
        public abstract double Eval(double t, double x);
    }

    public class GRegulazingKernel : IRegulazingKernel
    {
        private readonly int particlenumber;
        public GRegulazingKernel(int particlenumber)
        {
            this.particlenumber = particlenumber;
        }
        public override double Eval(double t, double x)
        {
            var bandwith = K * Math.Sqrt(Math.Max(Tmin, t)) * Math.Pow(particlenumber, -0.2);
            return 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(-Math.Pow(x / bandwith, 2)) / bandwith;
        }
    }

    public class QRegulazingKernel : IRegulazingKernel
    {
        private readonly int particlenumber;
        public QRegulazingKernel(int particlenumber)
        {
            this.particlenumber = particlenumber;
        }
        public override double Eval(double t, double x)
        {
            var bandwith = K * Math.Sqrt(Math.Max(Tmin, t)) * Math.Pow(particlenumber, -0.2);
            if (-1 < x && x < 1)
                return 0.9375 * Math.Pow(1 - Math.Pow(x / bandwith, 2), 2) / bandwith;
            return 0.0;
        }
    }
}
