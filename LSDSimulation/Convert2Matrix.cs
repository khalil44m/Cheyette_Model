using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;

namespace ClassLibrary1
{
    public class ConvertToMatrix
    {
        public static Matrix<double> Convert(double[,] array)
        {
            var nrow = array.GetLength(0);
            var ncol = array.GetLength(1);
            var matrix = Matrix<double>.Build.Dense(nrow, ncol);
            for (int i = 0; i < nrow; i++)
                for (int j = 0; j < ncol; j++)
                    matrix[i, j] = array[i, j];

            return matrix;
        }
    }

    public class ConvertToVector
    {
        public static Vector<double> Convert(double[] array)
        {
            var n = array.Length;
            var vector = Vector<double>.Build.Dense(n);
            for (int i = 0; i < n; i++) vector[i] = array[i];

            return vector;
        }
    }
}