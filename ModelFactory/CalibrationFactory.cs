using System;
using System.Collections.Generic;
using System.Text;
using LSDSimulation;
using Calibration;
using ClassLibrary1;
using MathNet.Numerics.Providers.LinearAlgebra;

namespace ModelFactory
{
    public class CalibrationFactory
    {

        public static ParticularCalibrator BuildCalibrator(ModelParameters modelParameters, CalibParameters calibParameters)
        {
            var simulator = new Simulator(LSDDiffusionParamatersFactory.BuildLSDDiffusionParameters(), modelParameters, MarketDataFactory.BuildMarketData(modelParameters));
            simulator.Step = simulator.ModelParameters.CalibrationStep;
            var kernel = new QRegulazingKernel(calibParameters.ParticleNumber);
            var calibrationgrid = new LocalCalibrationGrid(simulator.MarketData, simulator.ModelParameters.LocalTenor);
            var interpolatetype = InterpolationType.CubicSpline;

            return new ParticularCalibrator(simulator, kernel, calibParameters.ParticleNumber, calibrationgrid, interpolatetype);
        }
        /*public static ParticularCalibratorDiag BuildCalibratorDiag()
        {
            var simulator = new Simulator(LSDDiffusionParamatersFactory.BuildLSDDiffusionParameters(), GlobalVar.ModelParameters, MarketDataFactory.BuildMarketDataDiag(), GlobalVar.ModelParameters.CalibrationStep);
            var particlenumber = 12000;
            var kernel = new GRegulazingKernel(new GaussianKernel(), particlenumber);
            var calibrationgrid = new LocalCalibrationGrid(simulator.MarketData);
            var interpolatetype = InterpolationType.CubicSpline;

            return new ParticularCalibratorDiag(simulator, kernel, particlenumber, calibrationgrid, interpolatetype);
        }*/
    }
}