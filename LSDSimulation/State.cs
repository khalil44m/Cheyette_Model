using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSDSimulation;
using MathNet.Numerics.LinearAlgebra;

namespace ClassLibrary1
{
    public class State     // it's a node which contain all the factors (for every particle)
    {
        private readonly List<Factors> listFactors;
        public State(List<Factors> listFactors)
        {
            this.listFactors = listFactors;
        }
        public List<Factors> ListFactors => listFactors;
        public State NextState(Simulator simulator, Matrix<double> deterministiccorrelmatrix, double alpha)
        {
            var NParticle = listFactors.Count;
            var newlistFactors = new List<Factors>();
            for (int i = 0; i < NParticle; ++i) newlistFactors.Add(simulator.NextFactors(listFactors[i], deterministiccorrelmatrix, alpha));
            return new State(newlistFactors);
        }

        public static State InitialState(int factornumber, int particlenumber, MarketData marketdata)
        {
            var factors = Factors.InitialFactors(factornumber, marketdata);
            var initialList = new List<Factors>();

            for (int i = 0; i < particlenumber; ++i) initialList.Add(factors);
            return new State(initialList);
        }
        public double GetTime()
        {
            return listFactors[0].Time;
        }
    }
}