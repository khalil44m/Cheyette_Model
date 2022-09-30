using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;

namespace ClassLibrary1
{
    public interface ILocalVolSurface
    {
        double Eval(double t, double strike);
    }
    public class SampledLocalVolatility : ILocalVolSurface
    {
        private Dictionary<double, RRFunction> localvolslices;
        public SampledLocalVolatility(Dictionary<double, RRFunction> localvolslices)
        {
            this.localvolslices = localvolslices;
        }

        public Dictionary<double, RRFunction> LocalVolSlices
        {
            get => localvolslices;
            set => localvolslices = value;
        }
        public double Eval(double t, double strike)
        {
            if (localvolslices.TryGetValue(t, out var slice))
                return slice.Eval(strike);
            throw new ArgumentException("slice time not found");
        }
    }

    public class LocalVolatilitySurface : ILocalVolSurface
    {
        private R2RFunction localvolfunction;
        public LocalVolatilitySurface(R2RFunction localvolfunction)
        {
            this.localvolfunction = localvolfunction;
        }
        public R2RFunction LocalVolFunction
        {
            get => localvolfunction;
            set => localvolfunction = value;
        }
        public double Eval(double t, double strike)
        {
            return localvolfunction.Eval(t, strike);
        }
    }
}