using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ClassLibrary1;
using LSDSimulation;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Win32.SafeHandles;
// ReSharper disable All

namespace LSDSimulation
{

    public class Simulator
    {
        private LSDDiffusionParameters parameters;
        private readonly ModelParameters inputsparameters;
        private readonly MarketData marketdata;
        private readonly int factornumber;
        private readonly Matrix<double> choleskymatrix;
        private readonly Factors initialValue;
        private readonly Matrix<double> correlMatrix;
        public Simulator(LSDDiffusionParameters parameters, ModelParameters inputsparameters, MarketData marketData)
        {
            this.parameters = parameters;
            this.inputsparameters = inputsparameters;
            this.marketdata = marketData;
            choleskymatrix = inputsparameters.GetCholeskyMatrixCorrelation();
            factornumber = inputsparameters.FactorNumber;
            initialValue = Factors.InitialFactors(factornumber, marketdata);
            correlMatrix = Matrix<double>.Build.DenseIdentity(factornumber);
        }
        public LSDDiffusionParameters Parameters => parameters;
        public ModelParameters ModelParameters => inputsparameters;
        public MarketData MarketData => marketdata;
        public int FactorNumber => factornumber;
        public double Maturity => inputsparameters.Maturity;
        public double Expiry => inputsparameters.Expiry;
        public double Step { get; set; }
        public Factors InitialValue => initialValue;

        public Factors NextFactors(Factors currentFactors, Matrix<double> deterministiccorrelmatrix, double alpha)
        {
            var t = currentFactors.Time;
            var swaprate = currentFactors.SwapRate;
            var localvol = parameters.LocalVolatilitySurface.Eval(t, swaprate);

            var euler = EulerComponentsCalculator.EulerScheme(currentFactors, deterministiccorrelmatrix, choleskymatrix, localvol, alpha, Step, inputsparameters);
            var interestrate = marketdata.RatesMarket.InitialForwardRate.Eval(t + Step) + euler.Item1.Sum();
            var discountfactor = currentFactors.DiscountFactor * Math.Exp(-Step * interestrate);
            return new Factors(euler.Item1, euler.Item2, euler.Item3, interestrate, discountfactor, t + Step, currentFactors.SwapRate);
        }
    }
    public class Factors
    {
        private readonly Vector<double> x;
        private readonly Matrix<double> v;
        private readonly double stochasticvol;
        private readonly double interestrate;
        private readonly double discountfactor;
        private readonly double t;
        private double swaprate;
        public Factors(Vector<double> x, Matrix<double> v, double stochasticvol, double interestrate, double discountfactor, double t, double swaprate)
        {
            this.x = x;
            this.v = v;
            this.stochasticvol = stochasticvol;
            this.interestrate = interestrate;
            this.discountfactor = discountfactor;
            this.t = t;
            this.swaprate = swaprate;
        }
        public Vector<double> XFactor => x;
        public Matrix<double> VCrochet => v;
        public double StochasticVolatility => stochasticvol;
        public double InterestRate => interestrate;
        public double DiscountFactor => discountfactor;
        public double Time => t;
        public double SwapRate
        {
            get => swaprate;
            set => swaprate = value;
        }
        public static Factors InitialFactors(int n, MarketData marketdata)
        {
            Vector<double> x = Vector<double>.Build.Dense(n);
            Matrix<double> v = Matrix<double>.Build.Dense(n, n);
            double s = 1;
            double interestrate = marketdata.RatesMarket.InitialForwardRate.Eval(0); // Valeur initiale du taux forward f(0,0) à modifier;
            double discountfactor = 1;
            double t = 0;

            return new Factors(x, v, s, interestrate, discountfactor, t, 0);
        }
        public int GetFactors()
        {
            return x.Count;
        }
    }
}
