using System;
using System.Collections.Generic;
using System.Text;
using LSDSimulation;

namespace Calibration
{
    public class CalibParameters{

        private readonly int particleNumber;
        private readonly int pathNumber;

        public CalibParameters(int particleNumber, int pathNumber)
        {
            this.particleNumber = particleNumber;
            this.pathNumber = pathNumber;
        }

        public int ParticleNumber => particleNumber;
        public int PathNumber => pathNumber;


    }
    public static class MonteCarloOperator
    {
        public static double MCPriceSwaption(double[] numeraireCbbDoubles, double[] swaprateDoubles, double strike)
        {
            var n = swaprateDoubles.Length;
            var result = 0.0;
            for (var i = 0; i < n; i++) result += numeraireCbbDoubles[i] * (swaprateDoubles[i] - strike) * Heaviside.Eval(swaprateDoubles[i], strike);
            return result / n;
        }
    }
}