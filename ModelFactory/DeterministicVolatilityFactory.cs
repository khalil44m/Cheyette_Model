using System;
using System.Collections.Generic;
using System.Text;
using ClassLibrary1;
using LSDSimulation;

namespace ModelFactory
{
    public enum UseCase
    {
        Spread,
        Swaption
    }
    public abstract class DeterministicVolatilityFactory
    {
        public static DeterministicVolatilityFactory Create(UseCase useCase)
        {
            switch (useCase)
            {
                case UseCase.Spread:
                    return new SpreadParamDeterministicVolFactory();
                case UseCase.Swaption:
                    return new SwaptionParamDeterministicVolFactory();
                default:
                    throw new ArgumentException("not handled use case");
            }

        }

        public abstract DeterministicVolatility Build();
    }


    public class SpreadParamDeterministicVolFactory : DeterministicVolatilityFactory
    {
        public override DeterministicVolatility Build()
        {
            return null;
        }
    }
    public class SwaptionParamDeterministicVolFactory : DeterministicVolatilityFactory
    {
        public override DeterministicVolatility Build()
        {
            return null;
        }
    }
}