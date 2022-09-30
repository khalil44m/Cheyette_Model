using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class BinaryResearch
    {
        public static int Find(double x, double[] steps, double stepTolerance)
        {
            if (Math.Abs(steps[0] - x) < stepTolerance)
                return 0;

            if (x < steps[0])
                return 0;

            if (x >= steps[steps.Length - 1])
                return steps.Length - 1;

            int i = 0;
            int j = steps.Length;
            while (j - i > 1)
            {
                int k = (i + j) / 2;
                if (Math.Abs(x - steps[k]) <= stepTolerance)
                    return k;
                if (x < steps[k])
                    j = k;
                else
                    i = k;
            }
            return i;
        }
    }
}
