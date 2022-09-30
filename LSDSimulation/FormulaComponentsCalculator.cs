using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using ClassLibrary1;
using MathNet.Numerics.LinearAlgebra;

namespace LSDSimulation
{
    public class FormulaComponentsCalculator
    {
        private readonly ModelParameters inputparameters;
        private readonly RatesMarket ratesMarket;
        private readonly int factornumber;
        private readonly double maturity;
        private readonly double tenor;
        private readonly double a;
        private readonly double b;
        private readonly List<double> listtenor;

        public FormulaComponentsCalculator(ModelParameters inputparameters, RatesMarket ratesMarket, double tenor)
        {
            this.inputparameters = inputparameters;
            this.ratesMarket = ratesMarket;
            this.tenor = tenor;
            factornumber = inputparameters.FactorNumber;
            maturity = inputparameters.Maturity;
            a = inputparameters.A;
            b = inputparameters.B;
            listtenor = TimeGrid.RegularGrid(maturity, maturity + tenor, 1);
        }
        public Tuple<double[], double> EvalGammaZC(double T, Factors factors)
        {
            var t = factors.Time;
            //var h = inputparameters.Horizon;
            //var gammavectorh = new double[factornumber];
            //for (int i = 0; i < factornumber; i++) gammavectorh[i] = inputparameters.GammaFunctions[i].Eval(t, h);
            var zccurve = ratesMarket.InitialZeroCoupon;
            var gammavector = new double[factornumber];
            for (int i = 0; i < factornumber; i++) gammavector[i] = inputparameters.GammaFunctions[i].Eval(t, T);
            var zc = zccurve.Eval(T) * Math.Exp(VectorialCalculus.ScalarProduct(gammavector, factors.XFactor.ToArray()) - 0.5 * VectorialCalculus.ScalarProductMatrix(gammavector, factors.VCrochet.ToArray(), gammavector) /*+ VectorialCalculus.ScalarProductMatrix(gammavectorh, factors.VCrochet.ToArray(), gammavector)*/) / zccurve.Eval(t);
            return new Tuple<double[], double>(gammavector, zc);
        }
        public Tuple<double[,], double[]> EvalGammamatrixZc(Factors factors)
        {
            var gammamatrix = new double[factornumber, listtenor.Count - 1];
            var zcvector = new double[listtenor.Count - 1];
            for (var j = 0; j < listtenor.Count - 1; j++)
            {
                var pass = EvalGammaZC(listtenor[j + 1], factors);
                zcvector[j] = pass.Item2;
                var x = pass.Item1;
                for (var i = 0; i < factornumber; i++) gammamatrix[i, j] = x[i];
            }
            return new Tuple<double[,], double[]>(gammamatrix, zcvector);
        }
        public double EvalCbb(double[] zclist)
        {
            return zclist.Sum();
        }
        public double[] EvalDeriveCbb(double[,] matrixgamma, double[] zcvector)
        {
            return VectorialCalculus.VectorProductMatrix(matrixgamma, zcvector);
        }
        public double EvalSwapRate(double x1, double x2)
        {
            return a * (1 - x1) / x2 + b;
        }
        public double EvalDeriveSwapRate(double x1, double x2, double x3, double x4, double x5)
        {
            return (-a * x1 * x2 - (x3 - b) * x4) / x5;
        }
        public double EvalForwardRate(double T, double[] exp, double[] xfactor, double[] gamma, /*double[] gammah,*/ double[,] matrix)
        {
            return ratesMarket.InitialForwardRate.Eval(T) + VectorialCalculus.ScalarProduct(exp, xfactor) - VectorialCalculus.ScalarProductMatrix(exp, matrix, gamma) /*+ VectorialCalculus.ScalarProductMatrix(exp, matrix, gammah)*/;
        }
        public List<double> EvalCbbSwapRate(Factors factors)    // This function evaluates only the Swap Rate and the Numeraire Swap for Monte Carlo Simulation
        {
            var pass = EvalGammamatrixZc(factors);
            var zcvector = pass.Item2;
            var sizezcvector = zcvector.Length - 1;
            var cbb = EvalCbb(zcvector);
            var swaprate = EvalSwapRate(zcvector[sizezcvector], cbb);
            return new List<double> { cbb * factors.DiscountFactor, swaprate };
        }
        public Tuple<double[], double[]> GetCbbSwapRates(State state)    // Evaluation of the State
        {
            var listfactors = state.ListFactors;
            var n = listfactors.Count;
            var numeraireswap = new double[n];
            var swapratedoubles = new double[n];
            for (var i = 0; i < n; i++)
            {
                var components = EvalCbbSwapRate(listfactors[i]);
                numeraireswap[i] = components[0];
                swapratedoubles[i] = components[1];
                listfactors[i].SwapRate = components[1];
            }
            return new Tuple<double[], double[]>(numeraireswap, swapratedoubles);
        }
        // GetCbbSwapRates.Item1 returns BPV of the state (for each particle)
        // GetCbbSwapRates.Item2 returns Swap Rate of the state (for each particle)

        public Tuple<double, double, double, double[], double, double/*, double*/> EvalAllRates(Factors factors)   // This function evaluates all rates for Calibration
        {
            var pass = EvalGammamatrixZc(factors);
            var gammamatrix = pass.Item1;
            var zcvector = pass.Item2;
            var sizezcvector = zcvector.Length - 1;
            var cbb = EvalCbb(zcvector);
            var derivecbb = EvalDeriveCbb(gammamatrix, zcvector);
            var swaprate = EvalSwapRate(zcvector[sizezcvector], cbb);
            var TN = maturity + tenor;
            //var h = inputparameters.Horizon;
            var exp = new double[factornumber];
            var gamma = new double[factornumber]; //var gammah = new double[factornumber];
            var deriveswaparate = new double[factornumber];
            for (var i = 0; i < factornumber; i++)
            {
                exp[i] = inputparameters.GammaFunctions[i].Lambda * gammamatrix[i, sizezcvector] + 1;
                gamma[i] = gammamatrix[i, sizezcvector]; //gammah[i] = inputparameters.GammaFunctions[i].Eval(factors.Time, h);
                deriveswaparate[i] = EvalDeriveSwapRate(gammamatrix[i, sizezcvector], zcvector[sizezcvector], swaprate, derivecbb[i], cbb);
            }
            var interestrate = factors.InterestRate;
            var forwardrate = EvalForwardRate(TN, exp, factors.XFactor.ToArray(), gamma,/* gammah,*/factors.VCrochet.ToArray());
            var zcTN = zcvector[sizezcvector];
            var diff1 = interestrate - forwardrate * zcTN;
            var diff2 = diff1 - swaprate * cbb * ratesMarket.Instrument(tenor).InitialSwapFunction.Eval(factors.Time);
            //var zth = EvalGammaZC(inputparameters.Horizon, factors).Item2;
            return new Tuple<double, double, double, double[], double, double/*, double*/>(factors.DiscountFactor, cbb, swaprate, deriveswaparate, diff1, diff2/*, zth*/);
        }
        // EvalAllRates.Item1 returns the Discount Factor
        // EvalAllRates.Item2 returns the Coupon Bearing Bond (BPV)
        // EvalAllRates.Item3 returns the Swap Rate
        // EvalAllRates.Item4 returns the Swap Rate Derivative vector (for each factor)
        // EvalAllRates.Item5 returns r_t - f_(t,TN) * Z_(t,TN) (for calibration of the local volatility)
        // EvalAllRates.Item6 returns r_t - f_(t,TN) * Z_(t,TN) - R_t * B_t * A_t (for calibration of the deterministic volatility)
        // EvalAllRates.Item7 returns Z(t,Th)
        public Tuple<double, double, double, double[], double, double/*, double*/>[] GetAllRatesState(State state)
        {
            var listfactors = state.ListFactors;
            int n = listfactors.Count;
            var result = new Tuple<double, double, double, double[], double, double/*, double*/>[n];
            for (var i = 0; i < n; i++) result[i] = EvalAllRates(listfactors[i]);
            return result;
        }
        public VolatilitySurfaceFormulaComponents GetLocalComponents(State state, Tuple<double, double, double, double[], double, double/*, double*/>[] allratesstate, double[,] deterministiccorrelDoubles)
        {
            var listfactors = state.ListFactors;
            var n = listfactors.Count;
            var discount = new double[n]; var cbb = new double[n];
            /*var zth = new double[n];*/
            var swaprate = new double[n];
            var inconditionalterm = new double[n]; var others = new double[n];
            for (var i = 0; i < n; i++)
            {
                var componentscalculator = allratesstate[i];
                discount[i] = componentscalculator.Item1;
                cbb[i] = componentscalculator.Item2;
                //zth[i] = componentscalculator.Item7;
                swaprate[i] = componentscalculator.Item3;
                inconditionalterm[i] = componentscalculator.Item5;
                var squarestochasticvol = Math.Pow(listfactors[i].StochasticVolatility, 2);
                var deriveswap = componentscalculator.Item4;
                others[i] = squarestochasticvol * VectorialCalculus.ScalarProductMatrix(deriveswap, deterministiccorrelDoubles, deriveswap);
                listfactors[i].SwapRate = swaprate[i];   // Setting the Swap Rate of factors
            }
            return new VolatilitySurfaceFormulaComponents(discount, cbb,/* zth,*/ swaprate, inconditionalterm, others, null, null, null, state.GetTime());
        }
        public VolatilitySurfaceFormulaComponents GetLocalComponentsBis(State state, Tuple<double, double, double, double[], double, double/*, double*/>[] allratesstate)
        {
            var listfactors = state.ListFactors;
            var n = listfactors.Count;
            var discount = new double[n]; var cbb = new double[n]; /*var zth = new double[n];*/ var swaprate = new double[n]; var inconditionalterm = new double[n]; var others = new double[n];
            for (var i = 0; i < n; i++)
            {
                var componentscalculator = allratesstate[i];
                discount[i] = componentscalculator.Item1;
                cbb[i] = componentscalculator.Item2;
                //zth[i] = componentscalculator.Item7;
                swaprate[i] = componentscalculator.Item3;
                inconditionalterm[i] = componentscalculator.Item5;
                var squarestochasticvol = Math.Pow(listfactors[i].StochasticVolatility, 2);

                //var correlmatrix = Matrix<double>.Build.DenseIdentity(3);
                var deriveswap = componentscalculator.Item4;
                others[i] = squarestochasticvol * VectorialCalculus.ScalarProduct(deriveswap, deriveswap);
                listfactors[i].SwapRate = swaprate[i];   // Setting the Swap Rate of factors
            }
            return new VolatilitySurfaceFormulaComponents(discount, cbb,/* zth,*/ swaprate, inconditionalterm, others, null, null, null, state.GetTime());
        }
        public VolatilitySurfaceFormulaComponents GetDeterministicComponents(State state, double[] localvolDoubles)
        {
            var listfactors = state.ListFactors;
            var n = listfactors.Count;
            var discount = new double[n]; var cbb = new double[n]; /*var zth = new double[n];*/ var swaprate = new double[n]; var inconditionalterm = new double[n]; var others = new double[n]; var coeff0 = new double[n]; var coeff1 = new double[n]; var coeff2 = new double[n];
            var beta = inputparameters.Beta;
            var correlation = inputparameters.CorrelationMatrix;
            var rho12 = correlation[0, 1];
            var rho13 = correlation[0, 2];
            var rho23 = correlation[1, 2];

            var matrix1 = new double[3, 3];
            matrix1[0, 0] = -2 * rho12;
            matrix1[0, 1] = matrix1[1, 0] = rho12 - beta * rho13;
            matrix1[0, 2] = matrix1[2, 0] = beta * rho13 - rho12;
            matrix1[1, 1] = matrix1[2, 2] = matrix1[1, 2] = matrix1[2, 1] = 0.0;
            var matrix2 = new double[3, 3];
            matrix2[0, 0] = 1;
            matrix2[1, 1] = 1 + Math.Pow(beta, 2) - 2 * beta * rho23;
            matrix2[2, 2] = Math.Pow(beta, 2) + 1 - 2 * beta * rho23;
            matrix2[0, 1] = matrix2[1, 0] = beta * rho23 - 1;
            matrix2[0, 2] = matrix2[2, 0] = -beta * rho23 + 1;
            matrix2[1, 2] = matrix2[2, 1] = beta * (2 * rho23 - beta) - 1;
            for (var i = 0; i < n; i++)
            {
                var componentscalculator = EvalAllRates(listfactors[i]);
                discount[i] = componentscalculator.Item1;
                cbb[i] = componentscalculator.Item2;
                //zth[i] = componentscalculator.Item7;
                swaprate[i] = componentscalculator.Item3;
                inconditionalterm[i] = componentscalculator.Item6;
                others[i] = Math.Pow(listfactors[i].StochasticVolatility, 2) * Math.Pow(localvolDoubles[i], 2);

                var deriveswap = componentscalculator.Item4;
                var coeff = VolatilityComponentsCalculator.ComputePolynomialCoeff(deriveswap, matrix1, matrix2);
                coeff0[i] = coeff[0];   // c0_t
                coeff1[i] = coeff[1];   // c1_t
                coeff2[i] = coeff[2];   // c2_t
            }
            return new VolatilitySurfaceFormulaComponents(discount, cbb, /*zth,*/ swaprate, inconditionalterm, others, coeff0, coeff1, coeff2, state.GetTime());
        }
    }
}
