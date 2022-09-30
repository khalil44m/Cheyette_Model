using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using ClassLibrary1;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;


namespace ClassLibrary1
{
    public static class EulerComponentsCalculator
    {
        public static Vector<double> LambdaFactorVector(double[] lambdavector, Vector<double> x, double step)
        {
            var n = lambdavector.Length;
            var result = Vector<double>.Build.Dense(n);
            for (var i = 0; i < n; i++) result[i] = (1 - lambdavector[i] * step) * x[i];
            return result;
        }
        public static Matrix<double> LambdaFactorMatrix(double[] lambdavector, Matrix<double> v, double step)
        {
            var n = lambdavector.Length;
            var result = Matrix<double>.Build.Dense(n, n);
            for (var i = 0; i < n; i++)
                for (var j = 0; j <= i; j++)
                    result[i, j] = result[j, i] = (1 - (lambdavector[i] + lambdavector[j]) * step) * v[i, j];
            return result;
        }
        public static Matrix<double> BuildTransformationMatrix(double beta, double alpha) //F*Sigma
        {
            var result = Matrix<double>.Build.Dense(3, 3);
            result[0, 0] = 1.0;
            result[1, 1] = alpha;
            result[2, 2] = beta * alpha;
            result[0, 1] = -alpha;
            result[1, 2] = -result[2, 2];
            return result;
        }
        public static Tuple<Vector<double>, Matrix<double>, double> EulerScheme(Factors factors, Matrix<double> deterministiccorrelmatrix, Matrix<double> choleskymatrix, double l, double alpha, double step, ModelParameters modelparameters)
        {
            // Generate Brownian with correlation

            var gaussianvector = GaussianVector.GenerateGaussianVector(4);
            var correlbrownian = Math.Sqrt(step) * choleskymatrix * gaussianvector;

            // Euler Scheme for XFactors and VFactors

            var ones = Vector<double>.Build.Dense(3);
            for (var i = 0; i < 3; i++) ones[i] = 1.0;
            var x = factors.XFactor;
            var v = factors.VCrochet;
            var s = factors.StochasticVolatility;
            var lambdavector = modelparameters.GetLambdaVector();
            var beta = modelparameters.Beta;
            var lambdafactor = LambdaFactorVector(lambdavector, x, step);
            var lambdafactormatrix = LambdaFactorMatrix(lambdavector, v, step);
            var transformationmatrix = BuildTransformationMatrix(beta, alpha);

            Vector<double> nextxfactor = lambdafactor + step * v * ones + s * l * transformationmatrix * correlbrownian.SubVector(0, 3);
            Matrix<double> nextvfactor = lambdafactormatrix + step * Math.Pow(s * l, 2) * deterministiccorrelmatrix;

            // Euler Scheme for Stochastic Volatility

            var thetafunction = modelparameters.ThetaFunction;
            var nu = thetafunction.Nu;
            var gamma = thetafunction.Gamma;

            var nextstochasticvol = Math.Pow(s, 1 - nu * step) * Math.Exp(thetafunction.Eval(factors.Time) * step + gamma * correlbrownian[3]);

            return new Tuple<Vector<double>, Matrix<double>, double>(nextxfactor, nextvfactor, nextstochasticvol);
        }

        // Item1 returns the xfactor
        // Item2 returns the matrix v;
        // Item3 returns the stochastic volatility

        public static Tuple<Vector<double>, Matrix<double>, double> EulerSchemeBis(Factors factors, Matrix<double> correlmatrix, double l, double step, ModelParameters model)
        {
            var gaussianvector = GaussianVector.GenerateGaussianVector(4);
            var ones = Vector<double>.Build.Dense(3);
            for (var i = 0; i < 3; i++) ones[i] = 1.0;
            var x = factors.XFactor;
            var v = factors.VCrochet;
            var s = factors.StochasticVolatility;
            var lambdavector = model.GetLambdaVector();
            var lambdafactor = LambdaFactorVector(lambdavector, x, step);
            var lambdafactormatrix = LambdaFactorMatrix(lambdavector, v, step);


            Vector<double> nextxfactor = lambdafactor + step * v * ones + s * l * gaussianvector.SubVector(0, 3);
            Matrix<double> nextvfactor = lambdafactormatrix + step * Math.Pow(s * l, 2) * correlmatrix;


            var thetafunction = model.ThetaFunction;
            var nu = thetafunction.Nu;
            var gamma = thetafunction.Gamma;

            var nextstochasticvol = Math.Pow(s, 1 - nu * step) * Math.Exp(thetafunction.Eval(factors.Time) * step + gamma * gaussianvector[3]);

            return new Tuple<Vector<double>, Matrix<double>, double>(nextxfactor, nextvfactor, nextstochasticvol);
        }
    }
    public static class GaussianVector
    {
        public static Vector<double> GenerateGaussianVector(int size)
        {
            var result = Vector<double>.Build.Dense(size);
            var gaussianvector = new double[size];
            Normal.Samples(gaussianvector, 0, 1);
            for (var i = 0; i < size; i++) result[i] = gaussianvector[i];
            return result;
        }
    }
}