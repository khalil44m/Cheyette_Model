using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public static class PolynomialSquares
    {
        public static double GetPositiveSquare(double a, double b, double c)
        {
            var delta = Math.Pow(b, 2) - 4 * a * c;
            if (!(delta >= 0)) return 0.0;
            var result = (Math.Sqrt(delta) - b) / 2 * a;
            return result >= 0 ? result : 0.0;
        }
    }
}