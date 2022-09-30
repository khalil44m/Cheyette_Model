using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace ClassLibrary1
{
    public static class VolatilityComponentsCalculator
    {
        public static double[] ComputeDeterministicVolatility(double alpha, ModelParameters modelParameters)  // Compute d_i(t) for all i
        {
            var result = new double[3];
            var beta = modelParameters.Beta;
            var correlation = modelParameters.CorrelationMatrix;
            result[0] = Math.Sqrt(Math.Pow(alpha, 2) - 2 * beta * alpha + 1);
            result[1] = Math.Sqrt(1 + Math.Pow(beta, 2) - 2 * beta * correlation[1, 2]) * alpha;
            result[2] = beta * alpha;
            return result;
        }

        public static double[] ComputeDeterministicVolatilityLSV(ModelParameters modelParameters)  // Compute d_i(t) for all i
        {
            var result = new double[3];
            var beta = modelParameters.Beta;
            var correlation = modelParameters.CorrelationMatrix;
            result[0] = Math.Sqrt(-2 * beta + 2);
            result[1] = Math.Sqrt(1 + Math.Pow(beta, 2) - 2 * beta * correlation[1, 2]);
            result[2] = beta;
            return result;
        }
        public static Matrix<double> ComputeDeterministicVolCorrel(double alpha, ModelParameters modelParameters)   // Compute a_ij = d_i(t) * d_j(t) * c_ij(t)
        {
            var result = Matrix<double>.Build.Dense(3, 3);
            var deterministicvol = ComputeDeterministicVolatility(alpha, modelParameters);
            var beta = modelParameters.Beta;
            var correlation = modelParameters.CorrelationMatrix;
            var rho12 = correlation[0, 1];
            var rho13 = correlation[0, 2];
            var rho23 = correlation[1, 2];
            result[0, 0] = Math.Pow(deterministicvol[0], 2);
            result[1, 1] = Math.Pow(deterministicvol[1], 2);
            result[2, 2] = Math.Pow(deterministicvol[2], 2);
            result[0, 1] = result[1, 0] = (rho12 - beta * rho13) * alpha + (beta * rho23 - 1) * Math.Pow(alpha, 2);
            result[0, 2] = result[2, 0] = beta * alpha * (rho13 - rho23 * alpha);
            result[1, 2] = result[2, 1] = beta * Math.Pow(alpha, 2) * (rho23 - beta);
            return result;
        }
        public static Matrix<double> ComputeDeterministicVolCorrelLSV(ModelParameters modelParameters)   // Compute a_ij = d_i(t) * d_j(t) * c_ij(t)
        {
            var result = Matrix<double>.Build.Dense(3, 3);
            var deterministicvol = ComputeDeterministicVolatilityLSV(modelParameters);
            var beta = modelParameters.Beta;
            var correlation = modelParameters.CorrelationMatrix;
            var rho12 = correlation[0, 1];
            var rho13 = correlation[0, 2];
            var rho23 = correlation[1, 2];
            result[0, 0] = Math.Pow(deterministicvol[0], 2);
            result[1, 1] = Math.Pow(deterministicvol[1], 2);
            result[2, 2] = Math.Pow(deterministicvol[2], 2);
            result[0, 1] = result[1, 0] = (rho12 - beta * rho13) + (beta * rho23 - 1);
            result[0, 2] = result[2, 0] = beta * (rho13 - rho23);
            result[1, 2] = result[2, 1] = beta * (rho23 - beta);
            return result;
        }
        public static double[] ComputePolynomialCoeff(double[] deriveswap, double[,] matrix1, double[,] matrix2)   // Compute c0_t, c1_t, c2_t for deterministic volatility calibration
        {
            var result = new double[3];
            result[0] = Math.Pow(deriveswap[0], 2);
            result[1] = VectorialCalculus.ScalarProductMatrix(deriveswap, matrix1, deriveswap);
            result[2] = VectorialCalculus.ScalarProductMatrix(deriveswap, matrix2, deriveswap);
            return result;
        }
    }
}