using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class VolatilitySurfaceFormulaComponents
    {
        private readonly double[] discount;
        private readonly double[] cbb;
        //private readonly double[] zth;
        private readonly double[] swaprateDoubles;
        private readonly double[] inconditionalDoubles;
        private readonly double[] otherscomponents;
        private readonly double[] coeff0;
        private readonly double[] coeff1;
        private readonly double[] coeff2;
        private readonly double t;
        public VolatilitySurfaceFormulaComponents(double[] discount, double[] cbb, /*double[] zth,*/ double[] swaprateDoubles, double[] inconditionalDoubles, double[] otherscomponents, double[] coeff0, double[] coeff1, double[] coeff2, double t)
        {
            this.discount = discount;
            this.cbb = cbb;
            //this.zth = zth;
            this.swaprateDoubles = swaprateDoubles;
            this.inconditionalDoubles = inconditionalDoubles;
            this.otherscomponents = otherscomponents;
            this.coeff0 = coeff0;
            this.coeff1 = coeff1;
            this.coeff2 = coeff2;
            this.t = t;
        }
        public double[] Discount => discount;
        public double[] Cbb => cbb;
        //public double[] ZTH => zth;
        public double[] SwapRateDoubles => swaprateDoubles;
        public double[] InconditionalDoubles => inconditionalDoubles;
        public double[] OthersComponents => otherscomponents;
        public double[] Coeff0 => coeff0;
        public double[] Coeff1 => coeff1;
        public double[] Coeff2 => coeff2;
        public double Time => t;
        public int GetFactorsNb()
        {
            var n = discount.Length;
            return n;
        }
    }
}