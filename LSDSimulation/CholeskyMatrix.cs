using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace ClassLibrary1
{
    public static class CholeskyMatrix
    {
        public static Matrix<double> GetCholesky(Matrix<double> matrix)
        {
            Cholesky<double> chol = matrix.Cholesky();
            return chol.Factor;
        }
        public static Matrix<double> ConvertArray(double[,] matrix)
        {
            var nrow = matrix.GetLength(0);
            var ncol = matrix.GetLength(1);
            var m = Matrix<double>.Build.Dense(nrow, ncol);
            for (var i = 0; i < nrow; i++)
                for (var j = 0; j < ncol; j++)
                    m[i, j] = matrix[i, j];
            return m;
        }
    }
}