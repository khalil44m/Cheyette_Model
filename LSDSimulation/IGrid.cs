using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    interface IGrid
    {
    }

    public sealed class StandardGrid : IGrid
    {
        private readonly double[] points;
        public StandardGrid(double[] points)
        {
            this.points = points;
        }
        public double[] Points { get { return points; } }
    }

    public sealed class SampledSurface
    {
        private readonly double[] timeGrid;
        private readonly double[] spaceGrid;
        private readonly double[,] matrix;

        public SampledSurface(double[] timeGrid, double[] spaceGrid, double[,] matrix)
        {

            this.timeGrid = timeGrid;
            this.spaceGrid = spaceGrid;
            this.matrix = matrix;

        }
        public double[,] Matrix => matrix;
        public double[] TimeGrid => timeGrid;
        public double[] SpaceGrid => spaceGrid;
    }

    public class TimeGrid
    {
        public static List<double> RegularGrid(double start, double end, double step)
        {
            List<double> list = new List<double>();
            var n = (int)Math.Round((end - start) / step + 1);
            for (int i = 0; i < n; i++) list.Add(i * step + start);

            return list;
        }
    }
}