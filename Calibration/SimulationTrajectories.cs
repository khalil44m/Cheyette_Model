using System;
using System.Collections.Generic;
using ClassLibrary1;
using LSDSimulation;

namespace Calibration
{
    public class SimulationTrajectories
    {
        private readonly Simulator simulatorPricing;
        private readonly int pathnumber;
        private readonly List<double> timegrid;
        public SimulationTrajectories(Tuple<LocalVolatilitySurface, DeterministicVolatility> volatilitySurfaceSwaption, Simulator simulatorPricing, int pathnumber)
        {
            this.simulatorPricing = simulatorPricing;
            this.pathnumber = pathnumber;
            timegrid = TimeGrid.RegularGrid(0, simulatorPricing.Maturity, simulatorPricing.Step);
            simulatorPricing.Parameters.LocalVolatilitySurface = volatilitySurfaceSwaption.Item1;
            simulatorPricing.Parameters.DeterministicVolatility = volatilitySurfaceSwaption.Item2;
        }
        public SimulationTrajectories(LocalVolatilitySurface volatilitySurfaceSwaption, Simulator simulatorPricing, int pathnumber)
        {
            this.simulatorPricing = simulatorPricing;
            this.pathnumber = pathnumber;
            timegrid = TimeGrid.RegularGrid(0, simulatorPricing.Maturity, simulatorPricing.Step);
            simulatorPricing.Parameters.LocalVolatilitySurface = volatilitySurfaceSwaption;
        }

        public Tuple<double[], double[]> Simulate(UseMode useMode, double tenor)    // get the BPV and the swap rate of n paths at maturity T
        {
            var continuousalpha = simulatorPricing.Parameters.DeterministicVolatility;
            var factornumber = simulatorPricing.FactorNumber;
            var currentState = State.InitialState(factornumber, pathnumber, simulatorPricing.MarketData);
            var formulacomponents = new FormulaComponentsCalculator(simulatorPricing.ModelParameters, simulatorPricing.MarketData.RatesMarket, tenor);
            Tuple<double[], double[]> components = null;
            for (var i = 1; i < timegrid.Count; i++)
            {
                var alpha = (useMode == UseMode.LSD) ? continuousalpha.Eval(timegrid[i - 1]) : 1.0;
                var deterministiccorrel = VolatilityComponentsCalculator.ComputeDeterministicVolCorrel(alpha, simulatorPricing.ModelParameters);
                currentState = currentState.NextState(simulatorPricing, deterministiccorrel, alpha);
                components = formulacomponents.GetCbbSwapRates(currentState);
            }
            return components;
        }
    }

    public class SimulationTrajectoriesDiag
    {
        private readonly Simulator simulatorPricing;
        private readonly int pathnumber;
        private readonly List<double> timegrid;
        public SimulationTrajectoriesDiag(Tuple<LocalVolatilitySurface, DeterministicVolatility> volatilitySurfaceSwaption, Simulator simulatorPricing, int pathnumber)
        {
            this.simulatorPricing = simulatorPricing;
            this.pathnumber = pathnumber;
            timegrid = TimeGrid.RegularGrid(0, simulatorPricing.Maturity, simulatorPricing.Step);
            simulatorPricing.Parameters.LocalVolatilitySurface = volatilitySurfaceSwaption.Item1;
            simulatorPricing.Parameters.DeterministicVolatility = volatilitySurfaceSwaption.Item2;
        }

        public Tuple<double[], double[]> Simulate(double tenor)    // get the BPV and the swap rate of n paths at maturity T
        {
            var continuousalpha = simulatorPricing.Parameters.DeterministicVolatility;
            var factornumber = simulatorPricing.FactorNumber;
            var currentState = State.InitialState(factornumber, pathnumber, simulatorPricing.MarketData);

            var ratesmarket = simulatorPricing.MarketData.RatesMarket;

            var formulacomponents = new FormulaComponentsCalculator(simulatorPricing.ModelParameters, ratesmarket, tenor);
            Tuple<double[], double[]> components = null;
            for (var i = 1; i < timegrid.Count; i++)
            {
                var alpha = continuousalpha.Eval(timegrid[i - 1]);
                var deterministiccorrel = VolatilityComponentsCalculator.ComputeDeterministicVolCorrel(alpha, simulatorPricing.ModelParameters);
                currentState = currentState.NextState(simulatorPricing, deterministiccorrel, alpha);
                components = formulacomponents.GetCbbSwapRates(currentState);
            }
            return components;
        }
    }
}