using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using MathNet.Numerics.Distributions;

namespace ClassLibrary1
{
    public class ConverttoRRFunction : RRFunction
    {
        private readonly Func<double, double> function;
        public ConverttoRRFunction(Func<double, double> function)
        {
            this.function = function;
        }
        public override double Eval(double x)
        {
            return function(x);
        }
        public override bool HasFirstDerivative()
        {
            throw new NotImplementedException();
        }
        public override RRFunction GetFirstDerivative()
        {
            return new RRNumericalDerivative(new ConverttoRRFunction(function), 0.001);
        }
    }
    public class ConverttoR2RFunction : R2RFunction
    {
        private readonly Func<double, double, double> function;
        public ConverttoR2RFunction(Func<double, double, double> function)
        {
            this.function = function;
        }
        public override double Eval(double x, double y)
        {
            return function(x, y);
        }
        public override bool HasFirstPartialDerivative()
        {
            throw new NotImplementedException();
        }
        public override R2RFunction GetFirstPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(new ConverttoR2RFunction(function), 0.001, i).GetFirstPartialDerivative(i);
        }
        public override R2RFunction GetSecondPartialDerivative(int i)
        {
            return new R2RNumericalDerivative(new ConverttoR2RFunction(function), 0.001, i).GetSecondPartialDerivative(i);
        }
    }
}