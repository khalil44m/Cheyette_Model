using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary1;
using LSDSimulation;

namespace ModelFactory
{
    public class LSDDiffusionParamatersFactory
    {
        public static LSDDiffusionParameters BuildLSDDiffusionParameters()
        {
            var localvolslicedico = new Dictionary<double, RRFunction>
            {
                { 0, new ConstantRRFunction(0.007) }
            };

            var alphavolslicedico = new Dictionary<double, double>
            {
                { 0, 0 }
            };
            var initiallocalvol = new SampledLocalVolatility(localvolslicedico);
            var initialalphafunction = new SampledDeterministicVolatility(alphavolslicedico);
            return new LSDDiffusionParameters(initiallocalvol, initialalphafunction);
        }
    }
}
