using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.RootFinding;

namespace ClassLibrary1
{
    public abstract class R2RFunction
    {
        public abstract double Eval(double x, double y);
        public abstract bool HasFirstPartialDerivative();
        public abstract R2RFunction GetFirstPartialDerivative(int i);
        public abstract R2RFunction GetSecondPartialDerivative(int i);
        public static R2RFunction operator +(R2RFunction f, R2RFunction g)
        {
            return new Sum2Function(f, g);
        }
        public static R2RFunction operator +(R2RFunction f, double lambda)
        {
            return new Sum2Function(f, new Constant2Function(lambda));
        }
        public static R2RFunction operator *(R2RFunction f, R2RFunction g)
        {
            return new Product2Function(f, g);
        }
        public static R2RFunction operator *(R2RFunction f, double lambda)
        {
            return new Product2Function(f, new Constant2Function(lambda));
        }
        public static R2RFunction operator *(double lambda, R2RFunction f)
        {
            return f * lambda;
        }
    }
    public class Sum2Function : R2RFunction
    {
        private readonly R2RFunction f;
        private readonly R2RFunction g;

        public Sum2Function(R2RFunction f, R2RFunction g)
        {
            this.f = f;
            this.g = g;
        }

        public override double Eval(double x, double y)
        {
            return f.Eval(x, y) + g.Eval(x, y);
        }

        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return f.GetFirstPartialDerivative(i) + g.GetFirstPartialDerivative(i);
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstPartialDerivative()
        {
            return f.HasFirstPartialDerivative() && g.HasFirstPartialDerivative();
        }
    }
    public class Product2Function : R2RFunction
    {
        private readonly R2RFunction f;
        private readonly R2RFunction g;

        public Product2Function(R2RFunction f, R2RFunction g)
        {
            this.f = f;
            this.g = g;
        }
        public override double Eval(double x, double y)
        {
            return f.Eval(x, y) * g.Eval(x, y);
        }

        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return f.GetFirstPartialDerivative(i) * g + g.GetFirstPartialDerivative(i) * f;
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstPartialDerivative()
        {
            return f.HasFirstPartialDerivative() && g.HasFirstPartialDerivative();

        }
    }

    public class InverseR2RFunction : R2RFunction
    {
        private readonly R2RFunction f;
        public InverseR2RFunction(R2RFunction f)
        {
            this.f = f;
        }
        public override double Eval(double x, double y)
        {
            if (f.Eval(x, y) == 0.0)
            {
                throw new Exception("function cannot be inverted at this point");
            }
            return 1 / f.Eval(x, y);
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }
    public class Constant2Function : R2RFunction
    {
        private readonly double constant;

        public Constant2Function(double constant)
        {
            this.constant = constant;
        }
        public override double Eval(double x, double y)
        {
            return constant;
        }

        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return new Constant2Function(0.0);
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstPartialDerivative()
        {
            return true;
        }
    }

    public class IdentityYProjection : R2RFunction
    {
        public override double Eval(double x, double y)
        {
            return y;
        }
        public override bool HasFirstPartialDerivative()
        {
            return true;
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }
    public class SqrtFunction : R2RFunction
    {
        private readonly R2RFunction f;
        public SqrtFunction(R2RFunction f)
        {
            this.f = f;
        }
        public override double Eval(double x, double y)
        {
            if (f.Eval(x, y) < 0.0)
                return 0.0;
            return Math.Sqrt(f.Eval(x, y));
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }


    public sealed class PieceWiseR2RFunction : R2RFunction
    {

        private readonly SampledSurface sampledSurface;
        public PieceWiseR2RFunction(SampledSurface sampledSurface)
        {
            this.sampledSurface = sampledSurface;
        }

        public override double Eval(double t, double k)
        {
            var couple = SearchIndexes(t, k);
            return sampledSurface.Matrix[couple.Item1, couple.Item2];
        }

        private static Tuple<int, int> SearchIndexes(double t, double k)
        {
            var sampledTimes = new List<double>();
            var sampledSpace = new List<double>();
            var i = 0;
            var j = 0;
            while (t > sampledTimes[i] && i < sampledTimes.Count)
            {
                i++;
            }
            while (k > sampledSpace[j] && j < sampledSpace.Count)
            {
                j++;
            }

            return new Tuple<int, int>(i, j);
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class Interpolate_Time_Space : R2RFunction
    {
        private readonly List<double> timeGrid;
        private readonly List<List<double>> spaceGrid;
        private readonly List<List<double>> evaluation;
        private readonly R2RFunction initialfunction;
        public Interpolate_Time_Space(List<double> timeGrid, List<List<double>> spaceGrid, List<List<double>> evaluation, R2RFunction initialfunction)
        {
            this.timeGrid = timeGrid;
            this.spaceGrid = spaceGrid;
            this.evaluation = evaluation;
            this.initialfunction = initialfunction;
        }

        private int SearchIndex(double t)
        {
            var i = 0;
            while (t >= timeGrid[i + 1] && i < timeGrid.Count - 2)
            {
                i++;
            }

            return i;
        }
        public override double Eval(double t, double K)
        {
            var index = SearchIndex(t);     // l'indice i dans timeGrid
            if (index == 0)
                return initialfunction.Eval(0, K);
            return new Interpolation(spaceGrid[index - 1], evaluation[index - 1]).Eval(K);
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }

    public class R2RNumericalDerivative : R2RFunction
    {
        private readonly R2RFunction function;
        private readonly double epsilon;
        private readonly int i;
        public R2RNumericalDerivative(R2RFunction function, double epsilon, int i)
        {
            this.function = function;
            this.epsilon = epsilon;
            this.i = i;
        }
        public override double Eval(double x, double y)
        {
            if (i == 1) return 1 / (2 * epsilon) * (function.Eval(x + epsilon, y) - function.Eval(x - epsilon, y));
            if (i == 2)
                return 1 / (2 * epsilon) * (function.Eval(x, y + epsilon) - function.Eval(x, y - epsilon));
            throw new NotImplementedException("i should be 1 or 2");
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(function, epsilon, i);
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(GetFirstPartialDerivative(i), epsilon, i);
        }
    }

    public class StepR2RFunction : R2RFunction
    {
        private const double stepTolerance = 1.0e-7;
        private readonly Dictionary<double, RRFunction> dico;
        private readonly double[] steps;
        private readonly RRFunction[] values;
        public StepR2RFunction(Dictionary<double, RRFunction> dico)
        {
            this.dico = dico;
            steps = dico.Keys.ToArray();
            values = dico.Values.ToArray();
        }
        private int IndexOfStepBefore(double t)
        {
            if (Math.Abs(steps[0] - t) < stepTolerance)
                return 0;

            if (t < steps[0])
                return -1;

            if (t >= steps[steps.Length - 1])
                return steps.Length - 1;

            int i = 0;
            int j = steps.Length;
            while (j - i > 1)
            {
                int k = (i + j) / 2;
                if (Math.Abs(t - steps[k]) <= stepTolerance)
                    return k;
                if (t < steps[k])
                    j = k;
                else
                    i = k;
            }
            return i;
        }
        public override double Eval(double t, double strike)
        {
            var n = steps.Length;

            if (t <= steps[0] + stepTolerance)
                return values[0].Eval(strike);

            if (t >= steps[n - 1] - stepTolerance)
                return values[n - 1].Eval(strike);

            var index0 = IndexOfStepBefore(t);
            return values[index0].Eval(strike);
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }
    public class GammaFunction : R2RFunction
    {
        private readonly double lambda;

        public GammaFunction(double lambda)
        {
            this.lambda = lambda;
        }

        public double Lambda => lambda;


        public override double Eval(double t, double maturity)
        {
            if (lambda != 0) return (Math.Exp(-lambda * (maturity - t)) - 1) / lambda;
            return t - maturity;
        }
        public double EvalFirstDerivative(double t, double maturity)
        {
            return Eval(t, maturity) * lambda + 1;
        }

        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
    }
    public class SwaptionPriceBachelier : R2RFunction
    {
        private readonly RRFunction cbbfunction;  // à t= 0
        private readonly RRFunction swapfunction; // à t=0
        private readonly R2RFunction impliedVolatility; // on suppose que la vol est constante dans un premier temps !!
        private readonly double epsilon;

        public R2RFunction GetImpliedVolatility => impliedVolatility;
        public SwaptionPriceBachelier(RRFunction cbbfunction, RRFunction swapfunction, R2RFunction impliedVolatility, double epsilon)
        {
            this.cbbfunction = cbbfunction;
            this.swapfunction = swapfunction;
            this.impliedVolatility = impliedVolatility;
            this.epsilon = epsilon;
        }
        public double EvalImplied(double impliedvol, double t, double K)        // Evaluation en fonction de la vol impli
        {
            var nu = Math.Pow(impliedvol, 2) * t;
            if (nu == 0.0) throw new Exception("Divided by zero");
            var xT = (K - swapfunction.Eval(t)) / Math.Sqrt(nu);
            return cbbfunction.Eval(t) * Math.Sqrt(nu) * (Normal.PDF(0, 1, xT) - xT * (1 - Normal.CDF(0, 1, xT)));
        }
        public override double Eval(double t, double K)
        {
            var impliedvol = impliedVolatility.Eval(t, K);
            return EvalImplied(impliedvol, t, K);
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(new SwaptionPriceBachelier(cbbfunction, swapfunction, impliedVolatility, epsilon), epsilon, i).GetFirstPartialDerivative(i);
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(new SwaptionPriceBachelier(cbbfunction, swapfunction, impliedVolatility, epsilon), epsilon, i).GetSecondPartialDerivative(i);
        }
        public R2RFunction GetGlobalSquareSigmaLocFunction()
        {
            var price = new SwaptionPriceBachelier(cbbfunction, swapfunction, impliedVolatility, epsilon);
            var identity = new IdentityYProjection();
            return 2 * (price.GetFirstPartialDerivative(1) + identity * identity * price.GetFirstPartialDerivative(2) + -1 * identity * price);
        }
        public R2RFunction GetSquareSigmaLocFunction()
        {
            var price = new SwaptionPriceBachelier(cbbfunction, swapfunction, impliedVolatility, epsilon);
            var identity = new IdentityYProjection();
            var inverse = new InverseR2RFunction(price.GetSecondPartialDerivative(2));
            return 2 * (price.GetFirstPartialDerivative(1) + identity * identity * price.GetFirstPartialDerivative(2) + -1 * identity * price) * inverse;
        }
        public R2RFunction GetSigmaLocFunction()
        {
            return new SqrtFunction(GetSquareSigmaLocFunction());
        }
    }
    public class PriceSwaptionMonteCarlo : RRFunction
    {
        private readonly int simulationnumber;
        private readonly List<double> listswap;
        private readonly List<double> listcbb;
        private readonly double maturity;
        private readonly double rate;
        public PriceSwaptionMonteCarlo(int simulationnumber, double maturity, List<double> listswap, List<double> listcbb, double rate)
        {
            this.simulationnumber = simulationnumber;
            this.maturity = maturity;
            this.listswap = listswap;
            this.listcbb = listcbb;
            this.rate = rate;
        }
        public override double Eval(double K)
        {
            double result = 0.0;
            for (int i = 0; i < simulationnumber; i++)
            {
                result += listcbb[i] * (listswap[i] - K) * Heaviside.Eval(listswap[i], K);
            }

            return Math.Exp(-rate * maturity) * result / simulationnumber;
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
    public class ImpliedVolatilityMonteCarlo : R2RFunction
    {
        private readonly PriceSwaptionMonteCarlo priceMC;
        private readonly SwaptionPriceBachelier priceBachelier;
        public ImpliedVolatilityMonteCarlo(PriceSwaptionMonteCarlo priceMc, SwaptionPriceBachelier priceBachelier)
        {
            this.priceMC = priceMc;
            this.priceBachelier = priceBachelier;
        }

        public override double Eval(double t, double K)
        {
            var pricemc = priceMC.Eval(K);
            Func<double, double> func = x => priceBachelier.EvalImplied(x, t, K) - pricemc;
            double root = Brent.FindRoot(func, 0.002, 0.013, 1E-100, 1000000000);
            return root;
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            throw new NotImplementedException();
        }
    }
}