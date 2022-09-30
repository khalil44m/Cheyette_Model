using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using ClassLibrary1;
using MathNet.Numerics.LinearAlgebra;

namespace ClassLibrary1
{
    public class ModelParameters
    {
        private readonly double beta;
        private readonly Matrix<double> correlation;
        private readonly double[] lambdas;
        private readonly double nu;
        private readonly double gamma;
        private readonly int factornumber;
        private double maturity;
        private readonly double expiry;
        private readonly double horizon;
        private readonly double localtenor;
        private readonly double deterministictenor;
        private readonly double pricingstep;
        private readonly double calibrationstep;
        private readonly double a;
        private readonly double b;

        public ModelParameters(double beta, Matrix<double> correlation, double[] lambdas, double nu, double gamma, int factornumber, double maturity, double expiry, double horizon, double pricingstep, double calibrationstep, double localtenor, double deterministictenor, double a, double b)
        {
            this.beta = beta;
            this.correlation = correlation;
            this.lambdas = lambdas;
            this.nu = nu;
            this.gamma = gamma;
            this.factornumber = factornumber;
            this.maturity = maturity;
            this.expiry = expiry;
            this.horizon = horizon;
            this.pricingstep = pricingstep;
            this.calibrationstep = calibrationstep;
            this.localtenor = localtenor;
            this.deterministictenor = deterministictenor;
            this.a = a;
            this.b = b;
        }
        public Matrix<double> GetCholeskyMatrixCorrelation()
        {
            return CholeskyMatrix.GetCholesky(correlation);
        }
        public double[] GetLambdaVector() => lambdas;
        public double Beta => beta;
        public Matrix<double> CorrelationMatrix => correlation;
        public GammaFunction[] GammaFunctions => new[] { new GammaFunction(lambdas[0]), new GammaFunction(lambdas[1]), new GammaFunction(lambdas[2]) };
        public NuThetaFunction ThetaFunction => new NuThetaFunction(nu, gamma);
        public int FactorNumber => factornumber;
        public double Maturity
        {
            get => maturity;
            set => maturity = value;
        }
        public double Expiry => expiry;
        public double Horizon => horizon;
        public double LocalTenor => localtenor;
        public double PricingStep => pricingstep;
        public double CalibrationStep => calibrationstep;
        public double DeterministicTenor => deterministictenor;
        public double A => a;
        public double B => b;
    }
}