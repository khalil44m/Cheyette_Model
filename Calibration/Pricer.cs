using System;
using System.Collections.Generic;
using System.Text;
using LSDSimulation;
using ClassLibrary1;

namespace Calibration
{
    public enum UseMode
    {
        LSV,
        LSD
    }
    public class Pricer
    {
        private readonly SimulationTrajectories simulationTrajectories;
        private readonly SimulationTrajectoriesDiag simulationTrajectoriesDiag;
        private readonly double tenor;
        private readonly Tuple<double[], double[]> trajectories;
        public Pricer(UseMode useCase, SimulationTrajectories simulationTrajectories, double tenor)
        {
            this.simulationTrajectories = simulationTrajectories;
            this.tenor = tenor;
            trajectories = simulationTrajectories.Simulate(useCase, tenor);
        }
        public double Price(double strike)
        {
            return MonteCarloOperator.MCPriceSwaption(trajectories.Item1, trajectories.Item2, strike);
        }
        public SimulationTrajectories SimulateProcesses => simulationTrajectories;
        public SimulationTrajectoriesDiag SimulateProcessesDiag => simulationTrajectoriesDiag;
        public double Tenor => tenor;
    }
}