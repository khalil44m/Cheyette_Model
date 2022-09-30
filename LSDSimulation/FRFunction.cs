using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;

namespace ClassLibrary1
{
    public abstract class FRFunction
    {
        public abstract double Eval(Factors x);
        public abstract bool HasFirstDerivative();
        public abstract FRFunction GetFirstDerivative();
        public static FRFunction operator +(FRFunction f, FRFunction g)
        {
            return new SumFRFunction(f, g);
        }
        public static FRFunction operator +(FRFunction f, double lambda)
        {
            return new SumFRFunction(f, new ConstantFRFunction(lambda));
        }
        public static FRFunction operator -(FRFunction f, FRFunction g)
        {
            return f + (-1.0) * g;
        }
        public static FRFunction operator *(FRFunction f, FRFunction g)
        {
            return new ProductFRFunction(f, g);
        }
        public static FRFunction operator *(double lambda, FRFunction f)
        {
            return new ProductFRFunction(f, new ConstantFRFunction(lambda));
        }
        public static FRFunction operator *(FRFunction f, double lambda)
        {
            return lambda * f;
        }
        public static FRFunction operator /(FRFunction f, FRFunction g)
        {
            return new ProductFRFunction(f, new InverseFRFunction(g));
        }
    }
    public class SumFRFunction : FRFunction
    {
        private readonly FRFunction f;
        private readonly FRFunction g;
        public SumFRFunction(FRFunction f, FRFunction g)
        {
            this.f = f;
            this.g = g;
        }
        public override double Eval(Factors x)
        {
            return f.Eval(x) + g.Eval(x);
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative() && g.HasFirstDerivative();
        }
        public override FRFunction GetFirstDerivative()
        {
            return f.GetFirstDerivative() + g.GetFirstDerivative();
        }
    }
    public class ProductFRFunction : FRFunction
    {
        private readonly FRFunction f;
        private readonly FRFunction g;
        public ProductFRFunction(FRFunction f, FRFunction g)
        {
            this.f = f;
            this.g = g;
        }
        public override double Eval(Factors x)
        {
            return f.Eval(x) * g.Eval(x);
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative() && g.HasFirstDerivative();
        }
        public override FRFunction GetFirstDerivative()
        {
            return f * g.GetFirstDerivative() + f.GetFirstDerivative() * g;
        }
    }
    public class InverseFRFunction : FRFunction
    {
        private readonly FRFunction f;
        public InverseFRFunction(FRFunction f)
        {
            this.f = f;
        }
        public override double Eval(Factors x)
        {
            return 1 / f.Eval(x);
        }
        public override bool HasFirstDerivative()
        {
            return f.HasFirstDerivative();
        }
        public override FRFunction GetFirstDerivative()
        {
            throw new NotImplementedException();
        }
    }
    public class ConstantFRFunction : FRFunction
    {
        private readonly double constant;
        public ConstantFRFunction(double constant)
        {
            this.constant = constant;
        }
        public override double Eval(Factors x)
        {
            return constant;
        }
        public override bool HasFirstDerivative()
        {
            return true;
        }
        public override FRFunction GetFirstDerivative()
        {
            return new ConstantFRFunction(0);
        }
    }
}
