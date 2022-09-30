using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrary1;
using LSDSimulation;

namespace Calibration
{
    public static class ParticularOperators
    {
        public static double ConditionalExpectationFormula(VolatilitySurfaceFormulaComponents components, IRegulazingKernel kernel, double strike)
        {
            var numerator = 0.0;
            var denominator = 0.0;
            var n = components.GetFactorsNb();
            for (var i = 0; i < n; i++)
            {
                var kernelcalcul = kernel.Eval(components.Time, components.SwapRateDoubles[i] - strike);
                var cbbdiscount = components.Discount[i] * components.Cbb[i];
                numerator += cbbdiscount * components.OthersComponents[i] * kernelcalcul;
                denominator += cbbdiscount * kernelcalcul;
            }
            return numerator / denominator;
        }
        public static double[] KernelExpectationFormula(VolatilitySurfaceFormulaComponents components, IRegulazingKernel kernel, double strike)
        {
            var result = new double[3];
            var n = components.GetFactorsNb();
            for (var i = 0; i < n; i++)
            {
                var x = components.Discount[i] * components.Cbb[i] * kernel.Eval(components.Time, components.SwapRateDoubles[i] - strike) * components.OthersComponents[i] /*/ components.ZTH[i]*/;
                result[0] += x * components.Coeff0[i];
                result[1] += x * components.Coeff1[i];
                result[2] += x * components.Coeff2[i];
            }
            result[0] /= n;
            result[1] /= n;
            result[2] /= n;
            return result;
        }

        public static double InconditionalExpectationFormula(VolatilitySurfaceFormulaComponents components, double strike)
        {
            var result = 0.0;
            var n = components.GetFactorsNb();
            for (var i = 0; i < n; i++) result += components.Discount[i] * components.InconditionalDoubles[i] * Heaviside.Eval(components.SwapRateDoubles[i], strike) /*/ components.ZTH[i]*/;
            return result / n;
        }
        public static double InconditionalExpectationDiagFormula(VolatilitySurfaceFormulaComponents components, double strike)
        {
            var result = 0.0;
            var n = components.GetFactorsNb();
            for (var i = 0; i < n; i++) result += components.Discount[i] * components.SwapRateDoubles[i] * Heaviside.Eval(components.SwapRateDoubles[i], strike) /*/ components.ZTH[i]*/;
            return result / n;
        }
        //public static double ConditionalExpectationFormula(LocalVolFormulaComponents components, IRegulazingKernel kernel, double strike)
        //{
        //    var numerator = 0.0;
        //    var denominator = 0.0;
        //    var mult = 1.0;
        //    var n = components.GetFactorsNb();
        //    for (var i = 0; i < n; i++)
        //    {
        //        var kernelcalcul = kernel.Eval(components.Time, components.SwapaRateDoubles[i] - strike);
        //        foreach (var component in components.NumeratorComponents) mult *= component[i];
        //        mult *= kernelcalcul;
        //        numerator += mult;
        //        denominator += components.DenominatorComponents[i] * kernelcalcul;
        //    }
        //    return numerator / denominator;
        //}
        //public static double InconditionalExpectationFormula(LocalVolFormulaComponents components, double strike)
        //{
        //    var n = components.GetFactorsNb();
        //    var result = 0.0;
        //    for (var i = 0; i < n; i++) result += components.InconditionalComponents[i] * Heaviside.Eval(components.SwapaRateDoubles[i], strike);
        //    return result / n;
        //}
        //public static double KernelInconditionalExpectationFormula(LocalVolFormulaComponents components, IRegulazingKernel kernel, double strike)
        //{
        //    var n = components.GetFactorsNb();
        //    var result = 0.0;
        //    var mult = 1.0;
        //    for (var i = 0; i < n; i++)
        //    {
        //        foreach (var component in components.NumeratorComponents) mult *= component[i];
        //        result += kernel.Eval(components.Time, components.SwapaRateDoubles[i] - strike);
        //        result *= mult;
        //    }
        //    return result / n;
        //}

    }
}