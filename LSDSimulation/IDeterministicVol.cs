using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;

namespace ClassLibrary1
{
    public interface IDeterministicVol
    {
        double Eval(double t);
    }
    public class DeterministicVolatility : IDeterministicVol
    {
        private RRFunction alphafunction;
        public DeterministicVolatility(RRFunction alphafunction)
        {
            this.alphafunction = alphafunction;
        }
        public RRFunction AlphaFunction
        {
            get => alphafunction;
            set => alphafunction = value;
        }
        public double Eval(double t)
        {
            return alphafunction.Eval(t);
        }
    }

    public class SampledDeterministicVolatility : IDeterministicVol
    {
        private Dictionary<double, double> alphavolsample;
        public SampledDeterministicVolatility(Dictionary<double, double> alphavolsample)
        {
            this.alphavolsample = alphavolsample;
        }
        public Dictionary<double, double> DeterministicVolSample
        {
            get => alphavolsample;
            set => alphavolsample = value;
        }

        public double Eval(double t)
        {
            if (alphavolsample.TryGetValue(t, out var slice))
                return slice;
            throw new ArgumentException("slice time not found");
        }
    }
}