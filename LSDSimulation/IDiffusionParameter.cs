using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary1;
using MathNet.Numerics;
using MathNet.Numerics.Distributions;

// ReSharper disable once CheckNamespace
namespace LSDSimulation
{
    interface IDiffusionParameter
    {

    }

    public class LSDDiffusionParameters : IDiffusionParameter
    {
        private ILocalVolSurface localvolatilitySurface;
        private IDeterministicVol deterministicVol;
        public LSDDiffusionParameters(ILocalVolSurface localvolatilitySurface, IDeterministicVol deterministicVol)
        {
            this.deterministicVol = deterministicVol;
            this.localvolatilitySurface = localvolatilitySurface;
        }

        public ILocalVolSurface LocalVolatilitySurface
        {
            get => localvolatilitySurface;
            set => localvolatilitySurface = value;
        }
        public IDeterministicVol DeterministicVolatility
        {
            get => deterministicVol;
            set => deterministicVol = value;
        }
    }
}