using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ClassLibrary1
{
    public class VectorialCalculus
    {
        public static double ScalarProduct(double[] x, double[] y)
        {
            return x.Select((t, i) => t * y[i]).Sum();
        }
        public static double[] VectorProductMatrix(double[,] matrix, double[] x)
        {
            var nrow = matrix.GetLength(0);
            var ncol = matrix.GetLength(1);
            var result = new double[nrow];
            for (int i = 0; i < nrow; i++)
                for (int j = 0; j < ncol; j++)
                    result[i] += matrix[i, j] * x[j];
            return result;
        }
        public static double ScalarProductMatrix(double[] x, double[,] matrix, double[] y)
        {
            return x.Select((t1, i) => x.Select((t, j) => t1 * matrix[i, j] * y[j]).Sum()).Sum();
        }
    }
}