﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Win32.SafeHandles;
using MathNet.Numerics;
using MathNet.Numerics.RootFinding;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using ClassLibrary1;
using LSDSimulation;

namespace Calibration
{
    public enum ModelType
    {
        Lsv,
        Lsdv
    }

    public class ParticularCalibrator
    {
        private readonly Simulator simulator;
        private readonly IRegulazingKernel regulazingkernel;
        private readonly int particlenumber;
        private readonly ICalibrationGrid calibrationgrid;
        private readonly InterpolationType interpolationType;
        private readonly int factornumber;
        private readonly List<double> timegrid;
        private readonly double a;
        public ParticularCalibrator(Simulator simulator, IRegulazingKernel regulazingkernel, int particlenumber, ICalibrationGrid calibrationgrid, InterpolationType interpolationType)
        {
            this.simulator = simulator;
            this.regulazingkernel = regulazingkernel;
            this.particlenumber = particlenumber;
            this.calibrationgrid = calibrationgrid;
            this.interpolationType = interpolationType;
            var maturity = simulator.Maturity;
            var step = simulator.Step;
            factornumber = simulator.FactorNumber;
            timegrid = TimeGrid.RegularGrid(0, maturity, step);
            a = simulator.ModelParameters.A;
        }
        public Simulator Simulator => simulator;

        public Tuple<LocalVolatilitySurface, DeterministicVolatility> CalibrateVolatilitySurfaceSwaptionLSD()
        {
            // Initialize the local and deterministic volatility

            var currentState = State.InitialState(factornumber, particlenumber, simulator.MarketData);
            var localvolslicedico = new Dictionary<double, RRFunction>
            {
                { 0, new ConstantRRFunction(0.0065) }
            };
            var localvolatilityresult = new LocalVolatilitySurface(new StepR2RFunction(localvolslicedico));
            UpdateLocalVolatility(localvolatilityresult);

            var alphavolslicedico = new Dictionary<double, double>
            {
                { 0 , 0 }
            };
            var deterministicvolatilityresult = new DeterministicVolatility(new StepRRFunction(alphavolslicedico));
            UpdateDeterministicVolatility(deterministicvolatilityresult);

            var deterministiccorrel = VolatilityComponentsCalculator.ComputeDeterministicVolCorrel(alphavolslicedico[timegrid[0]], simulator.ModelParameters);

            // Particular Algorithm

            for (var i = 1; i < timegrid.Count; i++)
            {
                // Diffusion

                currentState = currentState.NextState(simulator, deterministiccorrel, alphavolslicedico[timegrid[i - 1]]);

                var localcalculator = new FormulaComponentsCalculator(simulator.ModelParameters, simulator.MarketData.RatesMarket, simulator.ModelParameters.LocalTenor);
                var allratestate = localcalculator.GetAllRatesState(currentState);
                var strikeATM = simulator.MarketData.RatesMarket.Instrument(simulator.ModelParameters.DeterministicTenor).InitialSwapFunction.Eval(timegrid[i]);  // A_t

                var deterministiccalculator = new FormulaComponentsCalculator(simulator.ModelParameters, simulator.MarketData.RatesMarket, simulator.ModelParameters.DeterministicTenor);
                var localvolDoubles = new double[particlenumber];
                for (var j = 0; j < particlenumber; j++) localvolDoubles[j] = localvolslicedico[timegrid[i - 1]].Eval(allratestate[j].Item3);
                var deterministiccomponents = deterministiccalculator.GetDeterministicComponents(currentState, localvolDoubles);

                var kernelcalculator = ParticularOperators.KernelExpectationFormula(deterministiccomponents, regulazingkernel, strikeATM);
                var inconditionalexpectation = ParticularOperators.InconditionalExpectationFormula(deterministiccomponents, strikeATM);
                var derivativetimeAtm = simulator.MarketData.AtmImpliedVolatilityMarket.DerivativeTime.Eval(timegrid[i]);
                var deterministicvolatility = PolynomialSquares.GetPositiveSquare(kernelcalculator[2], kernelcalculator[1], kernelcalculator[0] - 2 * (inconditionalexpectation + derivativetimeAtm));

                // Calibration of the deterministic volatility

                alphavolslicedico.Add(timegrid[i], deterministicvolatility);
                deterministicvolatilityresult = new DeterministicVolatility(new StepRRFunction(alphavolslicedico));
                UpdateDeterministicVolatility(deterministicvolatilityresult);

                // Calibration of the Local Volatility

                var listlocalvol = new List<double>();
                var strikeGrid = calibrationgrid.GridEval(timegrid[i]);
                deterministiccorrel = VolatilityComponentsCalculator.ComputeDeterministicVolCorrel(alphavolslicedico[timegrid[i]], simulator.ModelParameters);
                var localcomponents = localcalculator.GetLocalComponents(currentState, allratestate, deterministiccorrel.ToArray());

                foreach (var strike in strikeGrid)
                {
                    var condexpectation = ParticularOperators.ConditionalExpectationFormula(localcomponents, regulazingkernel, strike);
                    var incondexpectation = ParticularOperators.InconditionalExpectationFormula(localcomponents, strike);
                    var localvolatility = GetCalibrateLocalVolatility(timegrid[i], strike, condexpectation, incondexpectation);
                    listlocalvol.Add(localvolatility);
                }
                localvolslicedico.Add(timegrid[i], new InterpolateGrid(strikeGrid, listlocalvol, interpolationType));
                localvolatilityresult = new LocalVolatilitySurface(new StepR2RFunction(localvolslicedico));
                UpdateLocalVolatility(localvolatilityresult);
            }
            return new Tuple<LocalVolatilitySurface, DeterministicVolatility>(localvolatilityresult, deterministicvolatilityresult);
        }

        public LocalVolatilitySurface CalibrateVolatilitySurfaceSwaptionLSV()
        {
            var currentState = State.InitialState(factornumber, particlenumber, simulator.MarketData);
            var localvolslicedico = new Dictionary<double, RRFunction>
            {
                { 0, new ConstantRRFunction(0.0) }
            };
            var localvolatilityresult = new LocalVolatilitySurface(new StepR2RFunction(localvolslicedico));
            UpdateLocalVolatility(localvolatilityresult);

            var deterministiccorrel = VolatilityComponentsCalculator.ComputeDeterministicVolCorrel(1.0, simulator.ModelParameters);

            // Particular Algorithm

            for (var i = 1; i < timegrid.Count; i++)
            {
                // Diffusion

                currentState = currentState.NextState(simulator, deterministiccorrel, 1.0);
                var localcalculator = new FormulaComponentsCalculator(simulator.ModelParameters, simulator.MarketData.RatesMarket, simulator.ModelParameters.LocalTenor);
                var allratestate = localcalculator.GetAllRatesState(currentState);

                var localvolDoubles = new double[particlenumber];
                for (var j = 0; j < particlenumber; j++) localvolDoubles[j] = localvolslicedico[timegrid[i - 1]].Eval(allratestate[j].Item3);

                // Calibration of the Local Volatility

                var listlocalvol = new List<double>();
                var strikeGrid = calibrationgrid.GridEval(timegrid[i]);
                var localcomponents = localcalculator.GetLocalComponents(currentState, allratestate, deterministiccorrel.ToArray());

                foreach (var strike in strikeGrid)
                {
                    var condexpectation = ParticularOperators.ConditionalExpectationFormula(localcomponents, regulazingkernel, strike);
                    var incondexpectation = ParticularOperators.InconditionalExpectationFormula(localcomponents, strike);
                    var localvolatility = GetCalibrateLocalVolatility(timegrid[i], strike, condexpectation, incondexpectation);
                    listlocalvol.Add(localvolatility);
                }
                localvolslicedico.Add(timegrid[i], new InterpolateGrid(strikeGrid, listlocalvol, interpolationType));
                localvolatilityresult = new LocalVolatilitySurface(new StepR2RFunction(localvolslicedico));
                UpdateLocalVolatility(localvolatilityresult);
            }
            return localvolatilityresult;

        }
        public double GetCalibrateLocalVolatility(double time, double strike, double condexpectation, double incondexpectation)
        {
            var x = simulator.MarketData.ImpliedVolatilityMarket.SmileConvexity.Eval(time, strike);
            var y = simulator.MarketData.ImpliedVolatilityMarket.SigmaLocMarket.Eval(time, strike);
            var sigmaloc_2 = (2 * a * incondexpectation / x + y) / condexpectation;
            return sigmaloc_2 < 0 ? 0.0 : Math.Sqrt(sigmaloc_2);
        }
        public void UpdateLocalVolatility(LocalVolatilitySurface newLocalVolatilitySample)
        {
            simulator.Parameters.LocalVolatilitySurface = newLocalVolatilitySample;
        }
        public void UpdateDeterministicVolatility(DeterministicVolatility newDeterministicVolatilitySample)
        {
            simulator.Parameters.DeterministicVolatility = newDeterministicVolatilitySample;
        }
    }
}