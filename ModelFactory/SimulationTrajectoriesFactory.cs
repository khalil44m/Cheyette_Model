using System;
using System.Collections.Generic;
using System.Text;
using Calibration;
using ClassLibrary1;
using LSDSimulation;

namespace ModelFactory
{
    public class SimulationTrajectoriesFactory
    {
        public static SimulationTrajectories BuildTrajectories(UseMode useMode, ModelParameters parameters, CalibParameters calibparam)
        {
            var calibrator = CalibrationFactory.BuildCalibrator(parameters, calibparam);
            dynamic volatilitySurfaceSwaption = (useMode == UseMode.LSD) ? calibrator.CalibrateVolatilitySurfaceSwaptionLSD()
                    : volatilitySurfaceSwaption = calibrator.CalibrateVolatilitySurfaceSwaptionLSV();
            var simulatorpricing = calibrator.Simulator;
            simulatorpricing.Step = simulatorpricing.ModelParameters.PricingStep;

            return new SimulationTrajectories(volatilitySurfaceSwaption, simulatorpricing, calibparam.PathNumber);
        }
        /*public static SimulationTrajectoriesDiag BuildTrajectoriesDiag(int pathnumber)
        {
            var calibrator = CalibrationFactory.BuildCalibratorDiag();
            var volatilitySurfaceSwaption = calibrator.CalibrateVolatilitySurfaceSwaption();
            var simulatorpricing = calibrator.Simulator;
            return new SimulationTrajectoriesDiag(volatilitySurfaceSwaption, simulatorpricing, pathnumber);
        }*/
    }
}