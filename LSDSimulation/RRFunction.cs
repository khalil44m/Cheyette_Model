using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using MathNet.Numerics;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics.LinearAlgebra;


// ReSharper disable once CheckNamespace
namespace LSDSimulation
{
    public abstract class RRFunction
    {
        public abstract double Eval(double x);
        public abstract bool HasFirstDerivative();
        public abstract RRFunction GetFirstDerivative();
        public static RRFunction operator +(RRFunction f, RRFunction g)
        {
            return new SumRRFunction(f, g);
        }
        public static RRFunction operator +(RRFunction f, double lambda)
        {
            return new SumRRFunction(f, new ConstantRRFunction(lambda));
        }
        public static RRFunction operator -(RRFunction f, RRFunction g)
        {
            return f + (-1.0) * g;
        }
        public static RRFunction operator *(RRFunction f, RRFunction g)
        {
            return new ProductRRFunction(f, g);
        }
        public static RRFunction operator *(double lambda, RRFunction f)
        {
            return new ProductRRFunction(f, new ConstantRRFunction(lambda));
        }
        public static RRFunction operator *(RRFunction f, double lambda)
        {
            return lambda * f;
        }
        public static RRFunction operator /(RRFunction f, RRFunction g)
        {
            return new ProductRRFunction(f, new InverseRRFunction(g));
        }
    }

    public class SumRRFunction : RRFunction
    {
        private readonly RRFunction f;
        private readonly RRFunction g;
        public SumRRFunction(RRFunction f, RRFunction g)
        {
            this.f = f;
            this.g = g;
        }
        public override double Eval(double x)
        {
            return f.Eval(x) + g.Eval(x);
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative() && g.HasFirstDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            return f.GetFirstDerivative() + g.GetFirstDerivative();
        }
    }
    public class ProductRRFunction : RRFunction
    {
        private readonly RRFunction f;
        private readonly RRFunction g;
        public ProductRRFunction(RRFunction f, RRFunction g)
        {
            this.f = f;
            this.g = g;
        }
        public override double Eval(double x)
        {
            return f.Eval(x) * g.Eval(x);
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative() && g.HasFirstDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            return f * g.GetFirstDerivative() + f.GetFirstDerivative() * g;
        }
    }
    public class InverseRRFunction : RRFunction
    {
        private readonly RRFunction f;
        public InverseRRFunction(RRFunction f)
        {
            this.f = f;
        }
        public override double Eval(double x)
        {
            var alpha = f.Eval(x);
            if (alpha != 0) return 1 / f.Eval(x);
            throw new NotImplementedException("f(x) should be non-zero");
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }
    public class ConstantRRFunction : RRFunction
    {
        private readonly double constant;
        public ConstantRRFunction(double constant)
        {
            this.constant = constant;
        }
        public override double Eval(double x)
        {
            return constant;
        }
        public override bool HasFirstDerivative()
        {
            return true;
        }
        public override RRFunction GetFirstDerivative()
        {
            return new ConstantRRFunction(0);
        }
    }

    public class RRNumericalDerivative : RRFunction
    {
        private readonly RRFunction function;
        private readonly double epsilon;
        public RRNumericalDerivative(RRFunction function, double epsilon)
        {
            this.function = function;
            this.epsilon = epsilon;
        }
        public override double Eval(double x)
        {
            return 1 / (2 * epsilon) * (function.Eval(x + epsilon) - function.Eval(x - epsilon));
        }
        public override bool HasFirstDerivative()
        {
            return function.HasFirstDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            return new RRNumericalDerivative(function, epsilon);
        }
        public RRFunction GetSecondDerivative()
        {
            return new RRNumericalDerivative(GetFirstDerivative(), epsilon);
        }
    }

    public class StepRRFunction : RRFunction
    {
        private const double stepTolerance = 1.0e-7;
        private readonly Dictionary<double, double> dico;
        private readonly double[] steps;
        private readonly double[] values;
        public StepRRFunction(Dictionary<double, double> dico)
        {
            this.dico = dico;
            steps = dico.Keys.ToArray();
            values = dico.Values.ToArray();
        }
        public override double Eval(double x)
        {
            var n = steps.Length;

            if (x <= steps[0] + stepTolerance)
                return values[0];

            if (x >= steps[n - 1] - stepTolerance)
                return values[n - 1];

            var index = BinaryResearch.Find(x, steps, stepTolerance);
            return values[index];
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

    public class YProjectionFunction : RRFunction
    {
        private readonly R2RFunction r2RFunction;
        private readonly double x0;
        public YProjectionFunction(R2RFunction r2RFunction, double x0)
        {
            this.r2RFunction = r2RFunction;
            this.x0 = x0;
        }
        public override double Eval(double x)
        {
            return r2RFunction.Eval(x0, x);
        }
        public override bool HasFirstDerivative()
        {
            return r2RFunction.HasFirstPartialDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            return null;
        }
    }

    public class XProjectionFunction : RRFunction
    {
        private readonly R2RFunction r2RFunction;
        private readonly double y0;
        public XProjectionFunction(R2RFunction r2RFunction, double y0)
        {
            this.r2RFunction = r2RFunction;
            this.y0 = y0;
        }
        public override double Eval(double y)
        {
            return r2RFunction.Eval(y, y0);
        }
        public override bool HasFirstDerivative()
        {
            return r2RFunction.HasFirstPartialDerivative();
        }
        public override RRFunction GetFirstDerivative()
        {
            return null;
        }
    }

    public class PowerPolynomialFunction : RRFunction
    {
        private readonly int n;
        public PowerPolynomialFunction(int n)
        {
            this.n = n;
        }
        public override double Eval(double x)
        {
            return Math.Pow(x, n);
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

    public class GlobalParametrization : RRFunction
    {
        private readonly RRFunction[] basefunctions;
        private readonly double[] coefficients;
        public GlobalParametrization(RRFunction[] basefunctions, double[] coefficients)
        {
            this.basefunctions = basefunctions;
            this.coefficients = coefficients;
        }
        public RRFunction[] BaseFunctions => basefunctions;
        public double[] Coefficients => coefficients;
        public override double Eval(double x)
        {
            double result = 0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * basefunctions[i].Eval(x);
            }
            return result;
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

    public class LogarithmFunction : RRFunction
    {
        private readonly RRFunction f;
        public LogarithmFunction(RRFunction f)
        {
            this.f = f;
        }
        public override double Eval(double x)
        {
            if (f.Eval(x) > 0)
                return Math.Log(f.Eval(x));
            throw new NotImplementedException("x should be positive");
        }
        public override bool HasFirstDerivative()
        {
            return true;
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }

    public class ExponentialFunction : RRFunction
    {
        private readonly double origin;
        private readonly double slope;
        public ExponentialFunction(double origin, double slope)
        {
            this.origin = origin;
            this.slope = slope;
        }
        public override double Eval(double x)
        {
            return Math.Exp(origin + slope * x);
        }

        public override RRFunction GetFirstDerivative()
        {
            return new ExponentialFunction(origin + Math.Log(slope), slope);
        }

        public override bool HasFirstDerivative()
        {
            return true;
        }
    }

    public class NuThetaFunction : RRFunction       // Compute nu * theta in the stochastic volatility
    {
        public NuThetaFunction(double nu, double gamma)
        {
            this.Nu = nu;
            this.Gamma = gamma;
        }
        public double Nu { get; }
        public double Gamma { get; }
        public override double Eval(double t)
        {
            return -Math.Pow(Gamma, 2) / 2 * (1 + Math.Exp(-2 * Nu * t));
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

    public class ThetaFunction : RRFunction
    {
        private readonly double gamma;
        private readonly double nu;

        public ThetaFunction(double gamma, double nu)
        {
            this.gamma = gamma;
            this.nu = nu;
        }

        public double Nu => nu;
        public double Gamma => gamma;

        public override double Eval(double t)
        {
            return (-Math.Pow(gamma, 2) / 2 * nu) * (1 + Math.Exp(-2 * nu * t));
        }

        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }

        public override bool HasFirstDerivative()
        {
            return false;
        }
    }

    public class ThetaBisFunction : RRFunction
    {
        private readonly double gamma;
        private readonly double nu;

        public ThetaBisFunction(double gamma, double nu)
        {
            this.gamma = gamma;
            this.nu = nu;
        }

        public double Nu => nu;
        public double Gamma => gamma;

        public override double Eval(double t)
        {
            return (-Math.Pow(gamma, 2) / 2 * nu) * (1 - Math.Exp(-2 * nu * t));
        }

        public override RRFunction GetFirstDerivative()
        {
            var function = new ExponentialFunction(0, -2 * nu);
            return -Math.Pow(gamma, 2) * function;
        }

        public override bool HasFirstDerivative()
        {
            return true;
        }
    }

    public class GaussianKernel : RRFunction
    {
        public override double Eval(double x)
        {
            return 1 / Math.Sqrt(2 * Math.PI) * Math.Exp(-Math.Pow(x, 2));
        }
        public override bool HasFirstDerivative()
        {
            return true;
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }

    public class QuarticKernel : RRFunction
    {
        public override double Eval(double x)
        {
            if (-1 < x && x < 1)
                return 0.9375 * Math.Pow(1 - Math.Pow(x, 2), 2);
            return 0.0;
        }
        public override bool HasFirstDerivative()
        {
            return true;
        }
        public override RRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }

    public static class Heaviside
    {
        public static double Eval(double x, double K)
        {
            return x > K ? 1.0 : 0.0;
        }
    }
}