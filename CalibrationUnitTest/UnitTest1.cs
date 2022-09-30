using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using MathNet.Numerics;
using MathNet.Numerics.RootFinding;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra;
using Microsoft.Win32.SafeHandles;
using LSDSimulation;
using Microsoft.Office.Interop.Excel;
using NUnit.Framework.Api;
using ModelFactory;
using Calibration;
using ClassLibrary1;


namespace Runner
{
    public class Tests
    {
        [Test]
        public void RunCMS()
        {
            var useMode = UseMode.LSV;
            var output = OutPut.ATMvol;
            var corr = new[,] { { 1.0, 0.7, 0.5, 0 }, { 0.7, 1.0, 0.8, 0 }, { 0.5, 0.8, 1.0, 0 }, { 0, 0, 0, 1.0 } };
            var beta = 1.0;
            var lambdas = new[] { 0.3, 0.06, 0 };
            var nu = 0.01;
            var gamma = 0.1;
            var factornb = 3;
            var maturity = 1.0;
            var horizon = 20.0;
            var pricingstep = 1 / 252.0;
            var calibrationstep = 0.01;
            var localtenor = 10.0;
            var deterministictenor = 5.0;
            var a = 1.0;
            var b = 0.0001;
            var pathnumber = 32000;
            var particlenumber = 12000;

            Runner.Cms(useMode, output, beta, corr, lambdas, nu, gamma, factornb, maturity, horizon, pricingstep, calibrationstep, localtenor, deterministictenor, a, b, pathnumber, particlenumber);
        }
        [Test]
        public void RunDiag()
        {
            /*var corr = new[,] { { 1.0, 0.7, 0.5, 0.0 }, { 0.7, 1.0, 0.8, 0.0 }, { 0.5, 0.8, 1.0, 0.0 }, { 0.0, 0.0, 0.0, 1.0 } };
            var beta = 1.0;
            var lambdas = new[] { 0.3, 0.06, 0 };
            var nu = 0.01;
            var gamma = 0.1;
            var factornb = 3;
            var maturity = 0.75;
            var horizon = 20.0;
            var pricingstep = 1 / 252.0;
            var calibrationstep = 0.01;
            var localtenor = 5.0;
            var deterministictenor = 10.0;
            var a = 1.0;
            var b = 0.0001;
            var pathnumber = 32000;
            var particlenumber = 12000;

            Runner.Diag(beta, corr, lambdas, nu, gamma, factornb, maturity, horizon, pricingstep, calibrationstep, localtenor, deterministictenor, a, b, pathnumber, particlenumber);*/
        }

        [Test]
        public void ZCcurve()
        {
            var swaprates = Matrix<double>.Build.Dense(1, 1000);
            for (int k = 0; k < 1000; k++) swaprates[0, k] = MarketDataFactory.BuildRatesMarket().InitialZeroCoupon.Eval(0.05 * k);
            WriteExcel.Writedata("ZC", swaprates);
        }

        [Test]

        public void SwapRate()
        {
            var rates = Matrix<double>.Build.Dense(2, 1000);
            var ratesmarket = MarketDataFactory.BuildRatesMarket();
            for (int k = 0; k < 1000; k++)
            {
                rates[0, k] = ratesmarket.Instrument(5).InitialSwapFunction.Eval(0.05 * k);
                rates[1, k] = ratesmarket.Instrument(10).InitialSwapFunction.Eval(0.05 * k);
                //
            }
            WriteExcel.Writedata("Rates", rates);
        }
        /*[Test]
        public void Testing()
        {
        //LSD Parameters
        int factornumber = 3;
        var step = 0.01;  //pas de temps
        var theta = new ThetaFunction(0.0, 0.01);
        var thetabis = new ThetaBisFunction(0.001, 0.1);
        var gamma = new GammaFunction[factornumber];  //list des gamma function
        gamma[0] = new GammaFunction(0.01);
        gamma[1] = new GammaFunction(0.02);
        gamma[2] = new GammaFunction(0.09);
        var localvolfunction = new Constant2Function(0);
        var maturity = 1;
        var localvolatilitysurface = new LocalVolatilitySurface(localvolfunction);
        var deterministicvolfunction = new RRFunction[factornumber];
        deterministicvolfunction[0] = new ConstantRRFunction(1);
        deterministicvolfunction[1] = new ConstantRRFunction(1);
        deterministicvolfunction[2] = new ConstantRRFunction(1);
        var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
        var correlation = new RRFunction[3, 3];
        correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
        correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
        var correlationbrowian = new CorrelationBrownian(correlation);
        var stochasticvol = new StochasticVolatility(thetabis);
        var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);*/

        ///////////////////////////////////////////Simulator
        //var zccurve = new ZcCurve();
        //var tenor = 5;  // tenor fixé à 10 ans
        //var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
        //var cbbcurve = new CBBCurve(zccurve, tenor);
        //var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
        //var swapRate = new SwapRate(1, 0, couponbearingbond);
        //var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);

        ///////////////////////////////////////////Particular Simulator
        //var ParticleNb = 4096;
        //var bandwith = new Bandwidth(ParticleNb);
        //var quadraticKernel = new QuadraticKernel();
        //var gaussianKernel = new GaussianKernel();
        //var qRegulazing = new QRegulazingKernel(bandwith, quadraticKernel);
        //var gRegulazing = new GRegulazingKernel(bandwith, gaussianKernel);
        //var nu = 0.01;
        //var forwardcurve = new ForwardCurve(zccurve); // flat 
        //var forwardrate = new ForwardRate(forwardcurve, gamma, maturity, step);
        //var interestRate = new InterestRate(forwardrate);
        //var zcCurve = new ZcCurve();
        //var cbbCurve = new CBBCurve(zcCurve, tenor);
        //var swapCurve = new SwapCurve(cbbCurve);
        //var strikelist = new List<double> { -0.01, 0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1 };
        //var maturitylist = new List<double> { 1 / 12.0, 1 / 6.0, 0.25, 0.5, 0.75, 1, 2, 3, 4, 5, 7, 10 };
        //var list1 = new List<double> { 11.40858437, 8.890636591, 7.026993515, 4.545668106, 6.215533739, 8.589222289, 10.71152233, 13.87687376, 17.44042064, 21.00448624, 24.56855293, 28.13261962 }; //t= 1 mois
        //var list2 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 2 mois
        //var list3 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 3 mois
        //var list4 = new List<double> { 7.736979833, 6.749322846, 5.589425578, 4.329962481, 4.657898542, 6.04331126, 7.378084385, 8.515906536, 9.518751649, 10.42823389, 11.2654345, 12.12608614 }; // t = 6 mois
        //var list5 = new List<double> { 7.269991463, 6.384604708, 5.354337444, 4.30191068, 4.480666327, 5.646411145, 6.877247837, 7.966781575, 8.924344836, 9.78874527, 10.58392273, 11.32179681 }; // t = 9 mois
        //var list6 = new List<double> { 6.696042088, 5.94970959, 5.095744724, 4.272555619, 4.331997859, 5.237813637, 6.296913047, 7.291794223, 8.171277827, 8.964869375, 9.69368811, 10.37154898 }; // t = 1 an
        //var list7 = new List<double> { 5.8993877, 5.352107804, 4.751296036, 4.233247315, 4.237762222, 4.824939844, 5.636478355, 6.483447535, 7.295911497, 8.038928674, 8.718660592, 9.348949923 }; // t = 2 ans
        //var list8 = new List<double> { 5.620098457, 5.156367755, 4.660955348, 4.234258263, 4.195413907, 4.632440614, 5.321058267, 6.074210331, 6.827885978, 7.548840354, 8.217728641, 8.83597015 }; // t = 3 ans
        //var list9 = new List<double> { 5.409896741, 5.003545284, 4.581269677, 4.227884859, 4.177782289, 4.520261729, 5.125910504, 5.83856833, 6.579857825, 7.311337093, 8.009615428, 8.660496947 }; // t = 4 ans
        //var list10 = new List<double> { 5.151394844, 4.816617108, 4.479432829, 4.207833182, 4.166291101, 4.43183163, 4.940032066, 5.582260271, 6.280153353, 6.989486757, 7.68317602, 8.343997511 }; // t = 5 ans
        //var list11 = new List<double> { 5.158639422, 4.850781844, 4.513876414, 4.201314174, 4.13030725, 4.39137523, 4.875420987, 5.489809385, 6.176439221, 6.892115059, 7.608144705, 8.30643837 }; // t = 7 ans
        //var list12 = new List<double> { 4.643432108, 4.478836611, 4.269058979, 4.067229568, 4.029923453, 4.223569345, 4.581270215, 5.051875077, 5.621369812, 6.26554838, 6.94532192, 7.632906027 }; // t = 10 ans
        //var list1 = new List<double> { 12.70737206,  8.812812434, 6.046904567, 4.414243208, 4.146059715, 6.765803641, 10.66043085, 14.55506457, 17.91603131, 21.69801076, 25.4799902,  29.26196965 }; //t= 1 mois
        //var list2 = new List<double> { 8.891383941,  7.161351981, 5.874659068, 4.228652765, 4.051965097, 5.447797093, 7.468115921, 10.19183442, 12.54299362, 15.18895705, 17.83492048, 20.48088392 }; // t = 2 mois
        //var list3 = new List<double> { 7.996417037,  6.924625545, 5.661034161, 4.051591365, 3.924592444, 5.399732865, 6.653839069, 8.152389206, 10.00744996, 12.1163619,  14.22559516, 16.33483051 }; // t = 3 mois
        //var list4 = new List<double> { 7.456766534,  6.507662944, 5.394056183, 4.059803763, 3.869199004, 5.041529866, 6.286921861, 7.332163584, 8.245701557, 9.117566465, 10.16401171, 11.6127831 }; // t = 6 mois
        //var list5 = new List<double> { 7.091379547,  6.226170663, 5.219434342, 4.074660489, 3.915742673, 4.864606576, 6.044566527, 7.082484753, 7.986798553, 8.798840997, 9.54340443,  10.2597141 }; // t = 9 mois
        //var list6 = new List<double> { 6.64140437,   5.887786663, 5.022193642, 4.10077355,  3.952289768, 4.703720939, 5.726738073, 6.698769774, 7.550436988, 8.315597278, 9.016263493, 9.667027973 }; // t = 1 an
        //var list7 = new List<double> { 5.994527039,  5.42172552,  4.785099056, 4.17719189,  4.019947278, 4.486993293, 5.241367493, 6.060093703, 6.851394645, 7.572040397, 8.23001649,  8.839373223 }; // t = 2 ans
        //var list8 = new List<double> { 5.590500216,  5.138325867, 4.651877204, 4.208225301, 4.064800752, 4.366223617, 4.954875631, 5.638064927, 6.338674547, 7.015053742, 7.64316085,  8.223733066 }; // t = 3 ans
        //var list9 = new List<double> { 5.384252411,  4.997852368, 4.58956622,  4.215576889, 4.085872197, 4.323804568, 4.832422038, 5.468185311, 6.148025751, 6.82814487,  7.481486768, 8.09156903 }; // t = 4 ans
        //var list10 = new List<double> { 5.169016558, 4.850826061, 4.520088625, 4.219429365, 4.115851778, 4.311311987, 4.738932083, 5.306390816, 5.941680697, 6.598357638, 7.247037686, 7.868757606 }; // t = 5 ans
        //var list11 = new List<double> { 5.082618912, 4.785864592, 4.473346207, 4.178321069, 4.065698794, 4.248343715, 4.65446321,  5.195383591, 5.807544381, 6.449636759, 7.095732342, 7.72906482 }; // t = 7 ans
        //var list12 = new List<double> { 4.806105729, 4.576732347, 4.318923694, 4.061219554, 3.950866773, 4.094396431, 4.435182606, 4.899050412, 5.445732026, 6.0451172, 6.668478785, 7.295441545 }; // t = 10 ans

        //var impliedvollist = new List<List<double>> { list1, list2, list3, list4, list5, list6, list7, list8, list9, list10, list11, list12 };
        //foreach (var list in impliedvollist)
        //{
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        list[i] = list[i] * 16 * Math.Pow(10, -4);
        //    }
        //}
        //var impliedvol = new ImpliedVolatilityMarketInterpolation(maturitylist, strikelist, impliedvollist);
        //var epsilon = 0.0001;
        //var swaptionPriceBachelier = new SwaptionPriceBachelier(cbbCurve, swapCurve, impliedvol, epsilon);
        //var particularsimulator = new ParticularCalibrator(simulator, qRegulazing, nu, swapRate, interestRate, maturity, swaptionPriceBachelier, ParticleNb, gRegulazing, step, tenor);
        //var grid2DResults = new double[100, 100];
        //var grid1DResults = new double[100];
        //var localvolcalibrate = particularsimulator.Calibrate();
        //var localvolcalibrate_bis = particularsimulator.CalibratedLocalVol_bis();
        //var localvolapproxi = swaptionPriceBachelier.GetSigmaLocFunction();
        //var localvolapproximate = new LocalVolatilitySurface(maturity, localvolapproxi);
        //var local = localvolcalibrate.LocalVolFunction;
        //var currentState = State.InitialState(3, 38000);
        //var timegrid = TimeGrid.RegularGrid(0, maturity, 0.01);
        //particularsimulator.UpdateLocalVolatility(localvolcalibrate);
        //particularsimulator.UpdateLocalVolatility(localvolcalibrate_bis);
        //particularsimulator.UpdateLocalVolatility(localvolapproximate);
        //particularsimulator.UpdateStep(0.01);
        //for (int i = 0; i < timegrid.Count - 1; i++)
        //{
        //    currentState = currentState.NextState(simulator);
        //}
        //var listcbb = new List<double>();
        //var listswap = new List<double>();
        //var rate = 0.02;
        //for (int i = 0; i < 38000; i++)
        //{
        //    var factors = currentState.ListFactors[i];
        //    listswap.Add(swapRate.Eval(factors));
        //    listcbb.Add(swapRate.Cbb.Eval(factors));
        //}
        //var priceswaptionMC = new PriceSwaptionMonteCarlo(ParticleNb, maturity, listswap, listcbb, rate);
        //var impliedvolcalibration = new ImpliedVolatilityMonteCarlo(priceswaptionMC, swaptionPriceBachelier);
        //for (int i = 0; i < 25; i++)
        //{
        //Console.WriteLine(impliedvolcalibration.Eval(maturity, 0.01 + i * 0.0002));
        //Console.WriteLine(impliedvolcalibration.Eval(maturity, i * 0.001));
        //Console.WriteLine(priceswaptionMC.Eval(i * 0.001));
        //Console.WriteLine(impliedvolcalibration.Eval(maturity,  0.007 + i * 0.001));
        //}
        //for (int i = 0; i < 70; i++)
        //{
        //    //Console.WriteLine(impliedvolcalibration.Eval(maturity, -0.004 + i * 0.001));
        //    //Console.WriteLine(impliedvolcalibration.Eval(maturity, i * 0.001));
        //    //Console.WriteLine(impliedvolcalibration.Eval(maturity, -0.004 + i * 0.001));
        //    //Console.WriteLine(priceswaptionMC.Eval(i * 0.001));
        //    //Console.WriteLine(priceswaptionMC.Eval(-0.004 + i * 0.001));
        //}

        //var local_bis = particularsimulator.CalibratedLocalVol_bis().LocalVolFunction;
        //var tj = 1;
        //for (int i = 1; i < 100; ++i)
        //{
        //    //update
        //    var ki = i * 0.005; // update
        //    grid1DResults[i] = local_bis.Eval(tj, ki);
        //    Console.WriteLine(grid1DResults[i]);
        //}
        //Console.WriteLine(local_bis.Eval(0.02, 0.01));
        //Console.WriteLine(local.Eval(0.04, 0.001));
        //Console.WriteLine(local.Eval(0.04, 0.004));
        //Console.WriteLine(local.Eval(0.04, 0.0098));
        //Console.WriteLine(local.Eval(0.06, -0.0025));
        //Console.WriteLine(local.Eval(0.06, 0.0035));
        //Console.WriteLine(local.Eval(0.06, 0.01));
        //Console.WriteLine(local.Eval(0.5, -0.01));
        //Console.WriteLine(local.Eval(0.5, 0));
        //Console.WriteLine(local.Eval(0.5, 0.05));
        //Console.WriteLine(local.Eval(0.5, 0.08));
        //Console.WriteLine(local.Eval(0.5, 0.01));
        //Console.WriteLine(local.Eval(0.5, 0.011));
        //Console.WriteLine(local.Eval(0.5, 0.012));
        //Console.WriteLine(local.Eval(0.5, 0.013));
        //Console.WriteLine(local.Eval(0.5, 0.014));
        //for (int i = 0; i < 70; i++)
        //{
        //    //Console.WriteLine(local.Eval(2, -0.004 + i * 0.001));
        //    //Console.WriteLine(particularsimulator.SwaptionPriceMonteCarlo(0.02));
        //}
        //for (int i = 1; i < 51; i++)
        //{
        //    Console.WriteLine(local_bis.Eval(tj, i * 0.001));
        //}

        //Console.WriteLine(local.Eval(tj, 0.001));
        //Console.WriteLine(local.Eval(tj, 0.003));
        //Console.WriteLine(local.Eval(tj, 0.009));
        //Console.WriteLine(local.Eval(tj, 0.01));
        //Console.WriteLine(local.Eval(tj, 0.02));
        //Console.WriteLine(local.Eval(tj, 0.03));
        //Console.WriteLine(local.Eval(tj, 0.04));

        //var currentState = State.InitialState(3, 1024);
        //var nextstate = currentState.NextState(simulator);
        //nextstate = nextstate.NextState(simulator);
        //nextstate = nextstate.NextState(simulator);
        //Console.WriteLine(nextstate.ListFactors[2].XFactor[2]);

        //Console.WriteLine(local_bis.Eval(tj, 0.01 ));
        //Console.WriteLine(local_bis.Eval(tj, 0.33));

        //double result = local.Eval(0.6, 0.03);
        //Console.WriteLine(result);
        //double result_bis = local_bis.Eval(0.5, 0.01);
        //Console.WriteLine(local_bis.Eval(0.7, 0.02));
        //Console.WriteLine(result_bis);
        //Console.WriteLine(local_bis.Eval(0.35, 0.015));
    }
}
//[Test]
//public void TestApproximateLocalVolatility()
//{
///////////////////////////////////////////LSD Parameters
//    int factornumber = 3;
//    var step = 0.02; // pas de temps
//    var theta = new ThetaFunction(0.0, 0.01);
//    var thetabis = new ThetaBisFunction(0.01, 0.01);
//    var gamma = new GammaFunction[factornumber]; // list des gamma function
//    gamma[0] = new GammaFunction(0.01);
//    gamma[1] = new GammaFunction(0.02);
//    gamma[2] = new GammaFunction(0.09);
//    var localvolfunction = new Constant2Function(0.02);
//    var maturity = 1;
//    var localvolatilitysurface = new LocalVolatilitySurface(maturity, localvolfunction);
//    var deterministicvolfunction = new RRFunction[factornumber];
//    deterministicvolfunction[0] = new ConstantRRFunction(1);
//    deterministicvolfunction[1] = new ConstantRRFunction(1);
//    deterministicvolfunction[2] = new ConstantRRFunction(1);
//    var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
//    var correlation = new RRFunction[3, 3];
//    correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
//    correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
//    var correlationbrowian = new Correlationbrownian(correlation);
//    var stochasticvol = new StochasticVolatility(thetabis);
//    var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);

//    ///////////////////////////////////////////Simulator
//    var zccurve = new ZcCurve();
//    var tenor = 10;  // tenor fixé à 10 ans
//    var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
//    var cbbcurve = new CBBCurve(zccurve, tenor);
//    var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
//    var swapRate = new SwapRate(1, 0, couponbearingbond);
//    var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);

//    ///////////////////////////////////////////Particular Simulator
//    var ParticleNb = 1024;
//    var bandwith = new Bandwidth(ParticleNb);
//    var quadraticKernel = new QuadraticKernel();
//    var gaussianKernel = new GaussianKernel();
//    var qRegulazing = new QRegulazingKernel(bandwith, quadraticKernel);
//    var gRegulazing = new GRegulazingKernel(bandwith, gaussianKernel);
//    var nu = 1;
//    var forwardcurve = new ForwardCurve(zccurve); // flat 
//    var forwardrate = new ForwardRate(forwardcurve, gamma, maturity, step);
//    var interestRate = new InterestRate(forwardrate);
//    var zcCurve = new ZcCurve();
//    var cbbCurve = new CBBCurve(zcCurve, tenor);
//    var swapCurve = new SwapCurve(cbbCurve);
//    var strikelist = new List<double> { -0.01, 0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1 };
//    var maturitylist = new List<double> { 1 / 12.0, 1 / 6.0, 0.25, 0.5, 0.75, 1, 2, 3, 4, 5, 7, 10 };
//    var list1 = new List<double> { 11.40858437, 8.890636591, 7.026993515, 4.545668106, 6.215533739, 8.589222289, 10.71152233, 13.87687376, 17.44042064, 21.00448624, 24.56855293, 28.13261962 }; //t= 1 mois
//    var list2 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 2 mois
//    var list3 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 3 mois
//    var list4 = new List<double> { 7.736979833, 6.749322846, 5.589425578, 4.329962481, 4.657898542, 6.04331126, 7.378084385, 8.515906536, 9.518751649, 10.42823389, 11.2654345, 12.12608614 }; // t = 6 mois
//    var list5 = new List<double> { 7.269991463, 6.384604708, 5.354337444, 4.30191068, 4.480666327, 5.646411145, 6.877247837, 7.966781575, 8.924344836, 9.78874527, 10.58392273, 11.32179681 }; // t = 9 mois
//    var list6 = new List<double> { 6.696042088, 5.94970959, 5.095744724, 4.272555619, 4.331997859, 5.237813637, 6.296913047, 7.291794223, 8.171277827, 8.964869375, 9.69368811, 10.37154898 }; // t = 1 an
//    var list7 = new List<double> { 5.8993877, 5.352107804, 4.751296036, 4.233247315, 4.237762222, 4.824939844, 5.636478355, 6.483447535, 7.295911497, 8.038928674, 8.718660592, 9.348949923 }; // t = 2 ans
//    var list8 = new List<double> { 5.620098457, 5.156367755, 4.660955348, 4.234258263, 4.195413907, 4.632440614, 5.321058267, 6.074210331, 6.827885978, 7.548840354, 8.217728641, 8.83597015 }; // t = 3 ans
//    var list9 = new List<double> { 5.409896741, 5.003545284, 4.581269677, 4.227884859, 4.177782289, 4.520261729, 5.125910504, 5.83856833, 6.579857825, 7.311337093, 8.009615428, 8.660496947 }; // t = 4 ans
//    var list10 = new List<double> { 5.151394844, 4.816617108, 4.479432829, 4.207833182, 4.166291101, 4.43183163, 4.940032066, 5.582260271, 6.280153353, 6.989486757, 7.68317602, 8.343997511 }; // t = 5 ans
//    var list11 = new List<double> { 5.158639422, 4.850781844, 4.513876414, 4.201314174, 4.13030725, 4.39137523, 4.875420987, 5.489809385, 6.176439221, 6.892115059, 7.608144705, 8.30643837 }; // t = 7 ans
//    var list12 = new List<double> { 4.643432108, 4.478836611, 4.269058979, 4.067229568, 4.029923453, 4.223569345, 4.581270215, 5.051875077, 5.621369812, 6.26554838, 6.94532192, 7.632906027 }; // t = 10 ans
//    var impliedvollist = new List<List<double>> { list1, list2, list3, list4, list5, list6, list7, list8, list9, list10, list11, list12 };
//    foreach (var list in impliedvollist)
//    {
//        for (int i = 0; i < list.Count; i++)
//        {
//            list[i] = list[i] * 16 * Math.Pow(10, -4);
//        }
//    }
//    var impliedvol = new ImpliedVolatility(maturitylist, strikelist, impliedvollist);
//    var epsilon = 0.0001;
//    var swaptionPriceBachelier = new SwaptionPriceBachelier(cbbCurve, swapCurve, impliedvol, epsilon);
//    var particularsimulator = new ParticularCalibrator(simulator, qRegulazing, nu, swapRate, interestRate, maturity, swaptionPriceBachelier, ParticleNb, gRegulazing, step, tenor);
//    //var grid2DResults = new double[100, 100];
//    //var grid1DResults = new double[100];
//    //var localvolcalibrate = particularsimulator.CalibratedLocalVol();
//    var localvolapproxi = swaptionPriceBachelier.GetSigmaLocFunction();
//    var localvolapproximate = new LocalVolatilitySurface(maturity, localvolapproxi);
//    //var local = localvolcalibrate.LocalVolFunction;
//    var currentState = State.InitialState(3, 2000);
//    var initiallocalvol = new LocalVolatilitySurface(maturity,new Constant2Function(0.006));
//    particularsimulator.UpdateLocalVolatility(initiallocalvol);
//    currentState = currentState.NextState(simulator);
//    var timegrid = TimeGrid.RegularGrid(0, maturity, 0.02);
//    //particularsimulator.UpdateLocalVolatility(localvolcalibrate);
//    particularsimulator.UpdateLocalVolatility(localvolapproximate);
//    particularsimulator.UpdateStep(0.02);
//    for (int i = 1; i < timegrid.Count - 1; i++)
//    {
//        currentState = currentState.NextState(simulator);
//    }
//    var listcbb = new List<double>();
//    var listswap = new List<double>();
//    var rate = 0.02;
//    for (int i = 0; i < 2000; i++)
//    {
//        var factors = currentState.ListFactors[i];
//        listswap.Add(swapRate.Eval(factors));
//        listcbb.Add(swapRate.Cbb.Eval(factors));
//    }
//    var priceswaptionMC = new PriceSwaptionMonteCarlo(ParticleNb, maturity, listswap, listcbb, rate);
//    var impliedvolcalibration = new ImpliedVolatilityMonteCarlo(priceswaptionMC, swaptionPriceBachelier);
//    for (int i = 0; i < 70; i++)
//    {
//        Console.WriteLine(impliedvolcalibration.Eval(maturity, -0.004 + i * 0.001));
//        //Console.WriteLine(impliedvolcalibration.Eval(maturity, i * 0.001));
//        //Console.WriteLine(priceswaptionMC.Eval(i * 0.001));
//        //Console.WriteLine(priceswaptionMC.Eval(-0.004 + i * 0.001));
//    }
//}

//    [Test]
//    public void TestKernel()
//    {
//        var ParticleNb = 4000;
//        var bandwith = new Bandwidth(ParticleNb);
//        var quadraticKernel = new QuadraticKernel();
//        var gaussianKernel = new GaussianKernel();
//        var qRegulazing = new QRegulazingKernel(bandwith, quadraticKernel);
//        var gRegulazing = new GRegulazingKernel(bandwith, gaussianKernel);

//        //Console.WriteLine(qRegulazing.Eval(1, 0.01 - 0.0012));
//        //Console.WriteLine(bandwith.Eval(0.5));

//    }
//    [Test] public void TestZeroCoupon()
//    {
//        var listtime = TimeGrid.RegularGrid(0, 2, 100);
//        int factornumber = 3;
//        var theta = new ThetaFunction(0.5, 0.01);
//        var thetabis = new ThetaBisFunction(0.5, 0.01);
//        var gamma = new GammaFunction[factornumber]; // list des gamma function
//        gamma[0] = new GammaFunction(0.01);
//        gamma[1] = new GammaFunction(0.05);
//        gamma[2] = new GammaFunction(0.05);
//        var localvolfunction = new Constant2Function(0.01);
//        var maturity = 2;
//        var localvolatilitysurface = new LocalVolatilitySurface(maturity, localvolfunction);
//        var deterministicvolfunction = new RRFunction[factornumber];
//        deterministicvolfunction[0] = new ConstantRRFunction(1);
//        deterministicvolfunction[1] = new ConstantRRFunction(1);
//        deterministicvolfunction[2] = new ConstantRRFunction(1);
//        var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
//        var correlation = new RRFunction[3, 3];
//        correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
//        correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
//        var correlationbrowian = new Correlationbrownian(correlation);
//        var stochasticvol = new StochasticVolatility(thetabis);
//        var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);

//        ///////////////////////////////////////////Simulator
//        var zccurve = new ZcCurve(); // flat
//        var step = 1 / 10; // nombre de points du temps (t_i)
//        var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
//        var tenor = 10;  // tenor fixé à 10 ans
//        var cbbcurve = new CBBCurve(zccurve, tenor);
//        var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
//        var swapRate = new SwapRate(1, 0, couponbearingbond);
//        var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);
//        var forwardcurve = new ForwardCurve(zccurve); // flat 
//        var forwardrate = new ForwardRate(forwardcurve, gamma, maturity, step);
//        var interestRate = new InterestRate(forwardrate);
//        var factors = Factors.InitialFactors(factornumber);
//        var listfactors = new List<Factors>();
//        for (int i = 0; i < 100; i++)
//        {
//            factors = simulator.NextFactors(factors);
//            listfactors.Add(factors);
//        }
//        var tableauresult = new double[80];
//        //for (int i = 0; i < 70; i++)
//        //{
//        //    tableauresult[i] = zerocoupon.Eval(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //Console.WriteLine(zerocoupon.Eval(2,factors));
//        //for (int i = 0; i < 77; i++)
//        //{
//        //    tableauresult[i] = interestRate.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 77; i++)
//        //{ 
//        //    tableauresult[i] = forwardrate.Eval(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = listfactors[i].XFactor[1];
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = swapRate.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 77; i++)
//        //{
//        //    tableauresult[i] = swapRate.Derive(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = couponbearingbond.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}

//        ///////////////////////////////////////////
//        var ParticleNb = 1000;
//        var bandwith = new Bandwidth(ParticleNb);
//        var quadraticKernel = new QuadraticKernel();
//        var gaussianKernel = new GaussianKernel();
//        var qRegulazing = new QRegulazingKernel(bandwith, quadraticKernel);
//        var gRegulazing = new GRegulazingKernel(bandwith, gaussianKernel);
//        var nu = 0.001;
//        var zcCurve = new ZcCurve();
//        var cbbCurve = new CBBCurve(zcCurve, tenor);
//        var swapCurve = new SwapCurve(cbbCurve);
//        var impliedVolatility = new Constant2Function(0.008);
//        var epsilon = 0.0001;
//        var swaptionPriceBachelier = new SwaptionPriceBachelier(cbbCurve, swapCurve, impliedVolatility, epsilon);
//        var particularsimulator = new ParticularCalibrator(simulator, qRegulazing, nu, swapRate, interestRate, maturity, swaptionPriceBachelier, ParticleNb, gRegulazing, step, tenor);
//        var factorsini = simulator.InitialValue;
//        var initialList = new List<Factors>();
//        for (int i = 0; i < 1000; ++i) initialList.Add(factorsini);
//        var initialState = new State(initialList);
//        var listState = new List<State>();
//        listState.Add(initialState);
//        var initialsigmaLocFunction = new Constant2Function(0.01);
//        var localvolatility = new LocalVolatilitySurface(0.0, initialsigmaLocFunction);
//        particularsimulator.UpdateLocalVolatility(localvolatility);
//        //Console.WriteLine(listState[0].ListFactors[2].XFactor[2]);
//        //Console.WriteLine(swapRate.Cbb.ZeroCoupon.Eval(2.5, test));
//        //Console.WriteLine(zerocoupon.Eval(2.5, test));
//        //Console.WriteLine(swapRate.Eval(test));
//        for (int i = 1; i < 98; i++)
//        {
//        }
//        //var conditional = particularsimulator.ConditionalExpectation(listState, 0.02);
//        //var inconditional = particularsimulator.InconditionalExpectation(listState, 0.02);
//        //Console.WriteLine(conditional);
//        //Console.WriteLine(inconditional);
//        //Console.WriteLine(particularsimulator.LocalVolatility(0.2, 0.005, conditional, inconditional));

//    }

//    [Test]
//    public void TestSwapRate()
//    {
//        var step = 1 / 200;
//        var listtime = TimeGrid.RegularGrid(0, 2, step);
//        int factornumber = 3;
//        var theta = new ThetaFunction(0.5, 0.01);
//        var thetabis = new ThetaBisFunction(0.5, 0.01);
//        var gamma = new GammaFunction[factornumber]; // list des gamma function
//        gamma[0] = new GammaFunction(0.01);
//        gamma[1] = new GammaFunction(0.05);
//        gamma[2] = new GammaFunction(0.09);
//        var localvolfunction = new Constant2Function(0.8);
//        var maturity = 3;
//        var localvolatilitysurface = new LocalVolatilitySurface(maturity, localvolfunction);
//        var deterministicvolfunction = new RRFunction[factornumber];
//        deterministicvolfunction[0] = new ConstantRRFunction(1);
//        deterministicvolfunction[1] = new ConstantRRFunction(1);
//        deterministicvolfunction[2] = new ConstantRRFunction(1);
//        var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
//        var correlation = new RRFunction[3, 3];
//        correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
//        correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
//        var correlationbrowian = new Correlationbrownian(correlation);
//        var stochasticvol = new StochasticVolatility(thetabis);
//        var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);

//        ///////////////////////////////////////////Simulator
//        var zccurve = new ZcCurve(); // flat
//        var tenor = 10;
//        var cbbcurve = new CBBCurve(zccurve, tenor);
//        var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
//        var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
//        var swapRate = new SwapRate(1, 0, couponbearingbond);
//        var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);
//        var forwardcurve = new ForwardCurve(zccurve); // flat 
//        var forwardrate = new ForwardRate(forwardcurve, gamma, maturity, step);
//        var interestRate = new InterestRate(forwardrate);
//        var factors = Factors.InitialFactors(factornumber);
//        var listfactors = new List<Factors>();
//        for (int i = 0; i < 100; i++)
//        {
//            factors = simulator.NextFactors(factors);
//            listfactors.Add(factors);
//        }
//        var tableauresult = new double[80];
//        //for (int i = 0; i < 70; i++)
//        //{
//        //    tableauresult[i] = zerocoupon.Eval(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //Console.WriteLine(zerocoupon.Eval(2,factors));
//        //for (int i = 0; i < 77; i++)
//        //{
//        //    tableauresult[i] = interestRate.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 77; i++)
//        //{
//        //    tableauresult[i] = forwardrate.Eval(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = listfactors[i].XFactor[1];
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = swapRate.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 77; i++)
//        //{
//        //    tableauresult[i] = swapRate.Derive(2, listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//        //for (int i = 0; i < 60; i++)
//        //{
//        //    tableauresult[i] = couponbearingbond.Eval(listfactors[i]);
//        //    Console.WriteLine(tableauresult[i]);
//        //}
//    }
//    [Test]
//    public void TestStochasticVolatility()
//    {
//        var thetabis = new ThetaBisFunction(0.5, 0.01);
//        var stochasticvol = new StochasticVolatility(thetabis);
//        double result = stochasticvol.Eval(0);
//        Console.WriteLine(result);
//        Console.WriteLine(Math.Pow(2, 10));

//        // Avec la simulation, c'est toujours proche de 1

//    }
//    [Test]
//    public void TestSwaptionPrice()
//    {
//        var zcCurve = new ZcCurve();
//        var tenor = 10;
//        var cbbCurve = new CBBCurve(zcCurve, tenor);
//        var swapCurve = new SwapCurve(cbbCurve);
//        var impliedVolatility = new Constant2Function(0.08);
//        var epsilon = 0.0001;
//        var swaptionPriceBachelier = new SwaptionPriceBachelier(cbbCurve, swapCurve, impliedVolatility, epsilon);
//        //Console.WriteLine(swaptionPriceBachelier.Eval(4, 0.01));
//        //Console.WriteLine(swaptionPriceBachelier.Eval(4, 0.015));
//        //Console.WriteLine(swaptionPriceBachelier.GetFirstPartialDerivative(2).Eval(0.5, -0.01));
//        //Console.WriteLine(swaptionPriceBachelier.GetFirstPartialDerivative(2).Eval(0.5, 0.01));
//        //Console.WriteLine(swaptionPriceBachelier.GetFirstPartialDerivative(2).Eval(0.5, 0.02));
//        Console.WriteLine(swaptionPriceBachelier.GetFirstPartialDerivative(2).Eval(4.2, -0.2));
//        //Console.WriteLine(swaptionPriceBachelier.GetSecondPartialDerivative(2).Eval(0.02, 0.001));
//        //Console.WriteLine(swaptionPriceBachelier.GetSecondPartialDerivative(2).Eval(0.02, 0.005));
//        //Console.WriteLine(swaptionPriceBachelier.GetSecondPartialDerivative(2).Eval(0.04, 0.005));


//        //Console.WriteLine(swaptionPriceBachelier.GetSquareSigmaLocFunction().Eval(0.02, 0.01));
//        //Console.WriteLine(swaptionPriceBachelier.GetSquareSigmaLocFunction().Eval(0.06, 0.02));
//        //Console.WriteLine(swaptionPriceBachelier.GetSquareSigmaLocFunction().Eval(0.8, 0.015));
//        //Console.WriteLine(2 * 3 / 2 + 2);

//    }
//    [Test]
//    public void TestNumericalDerivative()
//    {
//        var function = new IdentityYProjection();
//        var Function = function * function;
//        var epsilon = 0.0001;
//        var deriv = new R2RNumericalDerivative(Function, epsilon, 2 );
//        Console.WriteLine(Function.Eval(2,3));
//        Console.WriteLine(deriv.GetFirstPartialDerivative(1).Eval(2, 5));
//        Console.WriteLine(deriv.Eval(2,5));
//        Console.WriteLine(deriv.GetFirstPartialDerivative(2).Eval(2, 5));
//        Console.WriteLine(deriv.GetSecondPartialDerivative(2).Eval(2,66));

//    }
// //    }
//    [Test]
//    public void Testinterpolation()
//    {
//        var X = new List<double>();
//        var Y = new List<double>();
//        for (int i = 0; i < 15; i++)
//        {
//            X.Add(i);
//            Y.Add(Math.Exp(i));
//        }
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_vector(X, Y)[0]);
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_vector(X, Y)[1]);
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_vector(X, Y)[2]);
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_vector(X, Y)[3]);
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_function(-50, X, Y));
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_function(2.5, X, Y));
//        //Console.WriteLine(Math.Exp(2.5));
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_function(3.5, X, Y));
//        //Console.WriteLine(Math.Exp(3.5));
//        //Console.WriteLine(Interpolate.cubic_spline_interpolate_function(7.8, X, Y));
//        //Console.WriteLine(Math.Exp(7.8));
//        Console.WriteLine(Math.Exp(0));
//        Console.WriteLine(Math.Exp(-0.025 * 0.02));
//        Console.WriteLine(Math.Exp(-0.025 * 0.04));
//        Console.WriteLine(Math.Exp(-0.025 * 0.06));
//        Console.WriteLine(Math.Exp(-0.025 * 0.08));
//        Console.WriteLine(Math.Exp(-0.025 * 0.1));
//        Console.WriteLine(Math.Exp(-0.025 * 0.12));
//        Console.WriteLine(Math.Exp(-0.025 * 0.14));
//        Console.WriteLine(Math.Exp(-0.025 * 0.16));
//        Console.WriteLine(Math.Exp(-0.025 * 0.18));

//    }
//    [Test]
//    public void TestState()
//    {
//        int factornumber = 3;
//        var step = 0.01; // pas de temps
//        var theta = new ThetaFunction(0.5, 0.01);
//        var thetabis = new ThetaBisFunction(0.5, 0.01);
//        var gamma = new GammaFunction[factornumber]; // list des gamma function
//        gamma[0] = new GammaFunction(0.01);
//        gamma[1] = new GammaFunction(0.05);
//        gamma[2] = new GammaFunction(0.09);
//        var localvolfunction = new Constant2Function(0.01);
//        var maturity = 1;
//        var localvolatilitysurface = new LocalVolatilitySurface(maturity, localvolfunction);
//        var deterministicvolfunction = new RRFunction[factornumber];
//        deterministicvolfunction[0] = new ConstantRRFunction(1);
//        deterministicvolfunction[1] = new ConstantRRFunction(1);
//        deterministicvolfunction[2] = new ConstantRRFunction(1);
//        var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
//        var correlation = new RRFunction[3, 3];
//        correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
//        correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
//        var correlationbrowian = new Correlationbrownian(correlation);
//        var stochasticvol = new StochasticVolatility(thetabis);
//        var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);

//        ///////////////////////////////////////////Simulator
//        var zccurve = new ZcCurve(); // flat
//        var tenor = 10;
//        var cbbcurve = new CBBCurve(zccurve, tenor);
//        var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
//        var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
//        var swapRate = new SwapRate(1, 0, couponbearingbond);
//        var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);
//        var currentState = State.InitialState(3, 1024);
//        for (int i = 0; i < 20; i++)
//        {
//            currentState = currentState.NextState(simulator);
//            Console.WriteLine(currentState.ListFactors[2].XFactor[2]);
//        }
//        Console.WriteLine(2 * 3);

//    }
//    [Test]
//    public void TestExcel()
//    {
//        //var zccurve = new ZcCurve();
//        //Console.WriteLine(zccurve.Eval(0));
//        double[] maturity = ReadExcel.readData("zerocoupon", 3);
//        Console.WriteLine(maturity[1]);
//        Console.WriteLine(1.2);
//    }

//    [Test]
//    public void TestImpliedVol()
//    {
//        var strikelist = new List<double> { -0.01, 0, 0.01, 0.02, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.09, 0.1 };
//        var maturitylist = new List<double> { 1 / 12.0, 1 / 6.0, 0.25, 0.5, 0.75, 1, 2, 3, 4, 5, 7, 10 };
//        //var list1 = new List<double> { 11.40858437, 8.890636591, 7.026993515, 4.545668106, 6.215533739, 8.589222289, 10.71152233, 13.87687376, 17.44042064, 21.00448624, 24.56855293, 28.13261962 }; //t= 1 mois
//        //var list2 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 2 mois
//        //var list3 = new List<double> { 9.8697492, 8.443479555, 6.720897878, 4.531730303, 5.701676236, 7.885281878, 9.617751993, 11.09196169, 12.59728323, 15.00162322, 17.54219006, 20.08661373 }; // t = 3 mois
//        //var list4 = new List<double> { 7.736979833, 6.749322846, 5.589425578, 4.329962481, 4.657898542, 6.04331126, 7.378084385, 8.515906536, 9.518751649, 10.42823389, 11.2654345, 12.12608614 }; // t = 6 mois
//        //var list5 = new List<double> { 7.269991463, 6.384604708, 5.354337444, 4.30191068, 4.480666327, 5.646411145, 6.877247837, 7.966781575, 8.924344836, 9.78874527, 10.58392273, 11.32179681 }; // t = 9 mois
//        //var list6 = new List<double> { 6.696042088, 5.94970959, 5.095744724, 4.272555619, 4.331997859, 5.237813637, 6.296913047, 7.291794223, 8.171277827, 8.964869375, 9.69368811, 10.37154898 }; // t = 1 an
//        //var list7 = new List<double> { 5.8993877, 5.352107804, 4.751296036, 4.233247315, 4.237762222, 4.824939844, 5.636478355, 6.483447535, 7.295911497, 8.038928674, 8.718660592, 9.348949923 }; // t = 2 ans
//        //var list8 = new List<double> { 5.620098457, 5.156367755, 4.660955348, 4.234258263, 4.195413907, 4.632440614, 5.321058267, 6.074210331, 6.827885978, 7.548840354, 8.217728641, 8.83597015 }; // t = 3 ans
//        //var list9 = new List<double> { 5.409896741, 5.003545284, 4.581269677, 4.227884859, 4.177782289, 4.520261729, 5.125910504, 5.83856833, 6.579857825, 7.311337093, 8.009615428, 8.660496947 }; // t = 4 ans
//        //var list10 = new List<double> { 5.151394844, 4.816617108, 4.479432829, 4.207833182, 4.166291101, 4.43183163, 4.940032066, 5.582260271, 6.280153353, 6.989486757, 7.68317602, 8.343997511 }; // t = 5 ans
//        //var list11 = new List<double> { 5.158639422, 4.850781844, 4.513876414, 4.201314174, 4.13030725, 4.39137523, 4.875420987, 5.489809385, 6.176439221, 6.892115059, 7.608144705, 8.30643837 }; // t = 7 ans
//        //var list12 = new List<double> { 4.643432108, 4.478836611, 4.269058979, 4.067229568, 4.029923453, 4.223569345, 4.581270215, 5.051875077, 5.621369812, 6.26554838, 6.94532192, 7.632906027 }; // t = 10 ans
//        var list1 = new List<double> { 12.70737206, 8.812812434, 6.046904567, 4.414243208, 4.146059715, 6.765803641, 10.66043085, 14.55506457, 17.91603131, 21.69801076, 25.4799902, 29.26196965 }; //t= 1 mois
//        var list2 = new List<double> { 8.891383941, 7.161351981, 5.874659068, 4.228652765, 4.051965097, 5.447797093, 7.468115921, 10.19183442, 12.54299362, 15.18895705, 17.83492048, 20.48088392 }; // t = 2 mois
//        var list3 = new List<double> { 7.996417037, 6.924625545, 5.661034161, 4.051591365, 3.924592444, 5.399732865, 6.653839069, 8.152389206, 10.00744996, 12.1163619, 14.22559516, 16.33483051 }; // t = 3 mois
//        var list4 = new List<double> { 7.456766534, 6.507662944, 5.394056183, 4.059803763, 3.869199004, 5.041529866, 6.286921861, 7.332163584, 8.245701557, 9.117566465, 10.16401171, 11.6127831 }; // t = 6 mois
//        var list5 = new List<double> { 7.091379547, 6.226170663, 5.219434342, 4.074660489, 3.915742673, 4.864606576, 6.044566527, 7.082484753, 7.986798553, 8.798840997, 9.54340443, 10.2597141 }; // t = 9 mois
//        var list6 = new List<double> { 6.64140437, 5.887786663, 5.022193642, 4.10077355, 3.952289768, 4.703720939, 5.726738073, 6.698769774, 7.550436988, 8.315597278, 9.016263493, 9.667027973 }; // t = 1 an
//        var list7 = new List<double> { 5.994527039, 5.42172552, 4.785099056, 4.17719189, 4.019947278, 4.486993293, 5.241367493, 6.060093703, 6.851394645, 7.572040397, 8.23001649, 8.839373223 }; // t = 2 ans
//        var list8 = new List<double> { 5.590500216, 5.138325867, 4.651877204, 4.208225301, 4.064800752, 4.366223617, 4.954875631, 5.638064927, 6.338674547, 7.015053742, 7.64316085, 8.223733066 }; // t = 3 ans
//        var list9 = new List<double> { 5.384252411, 4.997852368, 4.58956622, 4.215576889, 4.085872197, 4.323804568, 4.832422038, 5.468185311, 6.148025751, 6.82814487, 7.481486768, 8.09156903 }; // t = 4 ans
//        var list10 = new List<double> { 5.169016558, 4.850826061, 4.520088625, 4.219429365, 4.115851778, 4.311311987, 4.738932083, 5.306390816, 5.941680697, 6.598357638, 7.247037686, 7.868757606 }; // t = 5 ans
//        var list11 = new List<double> { 5.082618912, 4.785864592, 4.473346207, 4.178321069, 4.065698794, 4.248343715, 4.65446321, 5.195383591, 5.807544381, 6.449636759, 7.095732342, 7.72906482 }; // t = 7 ans
//        var list12 = new List<double> { 4.806105729, 4.576732347, 4.318923694, 4.061219554, 3.950866773, 4.094396431, 4.435182606, 4.899050412, 5.445732026, 6.0451172, 6.668478785, 7.295441545 }; // t = 10 ans
//        var impliedvollist = new List<List<double>> { list1, list2, list3, list4, list5, list6, list7, list8, list9, list10, list11, list12 };
//        foreach (var list in impliedvollist)
//        {
//            for (int i = 0; i < list.Count; i++)
//            {
//                list[i] = list[i] * 16 * Math.Pow(10, -4);
//            }
//        }
//        var zcCurve = new ZcCurve();
//        var tenor = 10;
//        var cbbCurve = new CBBCurve(zcCurve, tenor);
//        var swapCurve = new SwapCurve(cbbCurve);
//        var impliedvol = new ImpliedVolatility(maturitylist, strikelist, impliedvollist);
//        var impliedvol_bis = new Constant2Function(0.01);
//        var swaptionPrice = new SwaptionPriceBachelier(cbbCurve, swapCurve, impliedvol, 0.0001);
//        double alpha = 0.001;
//        double t = 10;
//        double coupon = cbbCurve.Eval(t);  // c'est le cbb à l'instant t=0
//        //Func<double, double> funcmax = x => swaptionPrice.GetFirstPartialDerivative(2).Eval(t, x) + alpha * coupon;
//        //Func<double, double> funcmin = x => swaptionPrice.GetFirstPartialDerivative(2).Eval(t, x) + (1 - alpha) * coupon;
//        //double max = Brent.FindRoot(funcmax, 0.0, 0.15, 0.00000001, 100000);
//        //double min = Brent.FindRoot(funcmin, -0.1, 0.05, 0.00000001, 100000);
//        //Console.WriteLine(max);
//        //Console.WriteLine(min);
//        //for (int i = 0; i < 20; i++)
//        //{
//        //    Console.WriteLine(Math.Exp(-0.02 * i));
//        //}
//        //var C = 0.0500948746833309;
//        //var k = 0.02;
//        //Func<double, double> funcmax = x => swaptionPrice. - C;
//        //double max = Brent.FindRoot(funcmax, 0.0, 0.03, 0.00000001, 1000);
//        //double min = Brent.FindRoot(funcmin, -0.1, 0.1, 0.00000001, 1000);

//        //var pricemc = 0.051871161;
//        //Func<double, double> func = x => swaptionPrice.EvalImplied(x, t, 0.02) - pricemc;
//        //double root = Brent.FindRoot(func, 0.0001, 0.02, 0.00000001, 1000);
//        //Console.WriteLine(root);
//        //Console.WriteLine(swaptionPrice.EvalImplied(0.010017849218055, t, -0.004));
//        //Console.WriteLine(impliedvol.Eval(0.0833333333, 0.015823331));
//        //Console.WriteLine(impliedvol.Eval(0.16666667, 0.015738486));
//        //Console.WriteLine(impliedvol.Eval(0.25, 0.015671864));
//        //Console.WriteLine(impliedvol.Eval(0.5, 0.015596813));
//        //Console.WriteLine(impliedvol.Eval(0.75, 0.015671471));
//        //Console.WriteLine(impliedvol.Eval(1, 0.015832668));
//        //Console.WriteLine(impliedvol.Eval(2, 0.016930165));
//        //Console.WriteLine(impliedvol.Eval(3, 0.018121341));
//        //Console.WriteLine(impliedvol.Eval(4, 0.019182906));
//        //Console.WriteLine(impliedvol.Eval(5, 0.020138401));
//        //Console.WriteLine(impliedvol.Eval(6, 0.020924395));
//        //Console.WriteLine(impliedvol.Eval(7, 0.021556463));
//        //Console.WriteLine(impliedvol.Eval(8, 0.022014179));
//        //Console.WriteLine(impliedvol.Eval(9, 0.022312755));
//        //Console.WriteLine(impliedvol.Eval(10, 0.022434191));
//        for (int i = 0; i < 55; i++)
//        {
//            //Console.WriteLine(swaptionPrice.Eval(0.5, i * 0.001));
//            //Console.WriteLine(swaptionPrice.EvalImplied(0.019094347, 1, -0.004));
//            //Console.WriteLine(impliedvol.Eval(2, -0.004 + i * 0.001));
//            //Console.WriteLine(swaptionPrice.Eval(0.5, -0.004));
//            /*Console.WriteLine(impliedvol.Eval(0.5, -0.004 + i * 0.001))*/
//            ;
//            //Console.WriteLine(swaptionPrice.GetSigmaLocFunction().Eval(2, -0.004 + i * 0.001));
//            //Console.WriteLine(swaptionPrice.Eval(1, -0.006 + i * 0.001));
//            //Console.WriteLine(swaptionPrice.GetSecondPartialDerivative(2).Eval(3, -0.01 + i * 0.001));
//            //Console.WriteLine(1);
//            Console.WriteLine(impliedvol.Eval(5, i * 0.001));
//            //Console.WriteLine(swaptionPrice.Eval(1, -0.01 + i * 0.001));
//        }
//        //Console.WriteLine(swaptionPrice.GetSecondPartialDerivative(2).Eval(3, -0.03));
//        //Console.WriteLine(swaptionPrice.Eval(1, -0.01));
//        //Console.WriteLine(swaptionPrice.GetFirstPartialDerivative(2).Eval(0.2, -12));
//        //Console.WriteLine(swaptionPrice.GetFirstPartialDerivative(2).Eval(0.2, 0.02));
//        //Console.WriteLine(1);
//        //Console.WriteLine(impliedvol.Eval(1, 0.1));
//        //Console.WriteLine(swaptionPrice.Eval(0.5, 0.02));
//        ////Console.WriteLine(swaptionPrice.Eval(5, 0.1001));
//        //Console.WriteLine(swaptionPrice.GetFirstPartialDerivative(2).Eval(3, 0.047));

//    }

//    [Test] public void TestSwaptionPriceMC()
//    {
//        var K = 0.02;
//        var particlenumber = 30000;
//        var step = 0.02;
//        var maturity = 1;
//        var currentState = State.InitialState(3, particlenumber);
//        var timegrid = TimeGrid.RegularGrid(0, maturity, step);
//        int factornumber = 3;
//        var theta = new ThetaFunction(0.0, 0.01);
//        var thetabis = new ThetaBisFunction(0.0, 0.01);
//        var gamma = new GammaFunction[factornumber]; // list des gamma function
//        gamma[0] = new GammaFunction(0.01);
//        gamma[1] = new GammaFunction(0.05);
//        gamma[2] = new GammaFunction(0.09);
//        var localvolfunction = new Constant2Function(0.006);
//        var localvolatilitysurface = new LocalVolatilitySurface(maturity, localvolfunction);
//        var deterministicvolfunction = new RRFunction[factornumber];
//        deterministicvolfunction[0] = new ConstantRRFunction(1);
//        deterministicvolfunction[1] = new ConstantRRFunction(1);
//        deterministicvolfunction[2] = new ConstantRRFunction(1);
//        var deterministicvolatility = new DeterministicVolatility(deterministicvolfunction);
//        var correlation = new RRFunction[3, 3];
//        correlation[0, 0] = correlation[1, 1] = correlation[2, 2] = new ConstantRRFunction(1);
//        correlation[0, 1] = correlation[0, 2] = correlation[1, 0] = correlation[2, 0] = correlation[1, 2] = correlation[2, 1] = new ConstantRRFunction(0);
//        var correlationbrowian = new Correlationbrownian(correlation);
//        var stochasticvol = new StochasticVolatility(thetabis);
//        var parameters = new LSDDiffusionParameters(localvolatilitysurface, deterministicvolatility, correlationbrowian, gamma, theta, stochasticvol);

//        ///////////////////////////////////////////Simulator
//        var zccurve = new ZcCurve();
//        var tenor = 10;  // tenor fixé à 10 ans
//        var zerocoupon = new ZeroCoupon(zccurve, gamma, maturity, step);
//        var cbbcurve = new CBBCurve(zccurve, tenor);
//        var couponbearingbond = new CouponBearingBond(zerocoupon, cbbcurve);
//        var swapRate = new SwapRate(1, 0, couponbearingbond);
//        var simulator = new Simulator(parameters, factornumber, maturity, swapRate, step);
//        for (int i = 0; i < timegrid.Count - 1; i++)
//        {
//            currentState = currentState.NextState(simulator);
//        }
//        double result = 0;
//        for (int i = 0; i < particlenumber; i++)
//        {
//            var factors = currentState.ListFactors[i];
//            var swaprate = swapRate.Eval(factors);
//            var cbb = swapRate.Cbb.Eval(factors);
//            result += cbb * (swaprate - K) * Heaviside.Eval(swaprate, K);
//        }
//        Console.WriteLine(Math.Exp(-0.02 * maturity) * result / 30000);
//        //Console.WriteLine();
//    }
//    [Test]
//    public void TestZCCurve()
//    {
//        var zccurve = new ZcCurve();
//        for (int i = 0; i < 200; i++)
//        {
//            Console.WriteLine(zccurve.Eval(i * 0.02));
//        }
//    }
//    [Test]
//    public void TestForwardCurve()
//    {
//        var zccurve = new ZcCurve();
//        var forwardcurve = new ForwardCurve(zccurve);
//        for (int i = 0; i < 200; i++)
//        {
//            Console.WriteLine(forwardcurve.Eval(i * 0.02));
//        }
//    }
//    [Test]
//    public void TestSwapCurve()
//    {
//        var tenor = 5;
//        var zcCurve = new ZcCurve();
//        var cbbCurve = new CBBCurve(zcCurve, tenor);
//        var swapCurve = new SwapCurve(cbbCurve);
//        Console.WriteLine(swapCurve.Eval(0.0833333333));
//        Console.WriteLine(swapCurve.Eval(0.16666667));
//        Console.WriteLine(swapCurve.Eval(0.25));
//        Console.WriteLine(swapCurve.Eval(0.5));
//        Console.WriteLine(swapCurve.Eval(0.75));
//        Console.WriteLine(swapCurve.Eval(1));
//        Console.WriteLine(swapCurve.Eval(2));
//        Console.WriteLine(swapCurve.Eval(3));
//        Console.WriteLine(swapCurve.Eval(4));
//        Console.WriteLine(swapCurve.Eval(5));
//        Console.WriteLine(swapCurve.Eval(6));
//        Console.WriteLine(swapCurve.Eval(7));
//        Console.WriteLine(swapCurve.Eval(8));
//        Console.WriteLine(swapCurve.Eval(9));
//        Console.WriteLine(swapCurve.Eval(10));
//        Console.WriteLine(1);
//    }
//    [Test]
//    public void TestStandardDeviation()
//    {
//        var list = new List<double>{0.01,0.02,0.025,0.03,0.04,0.015,0.06};
//        Console.WriteLine(Probability.StdDev(list,false));
//    }
//    [Test]
//    public void TestSelectIndex()
//    {
//        var list = new List<double>(4);
//        list.Add(1.5);
//        list.Add(2.2);
//        list.Add(3);
//        list.Add(3.5);
//        var selectlist = list.Where(x => x > 3.2).Select(x => list.IndexOf(x));
//        for (int i = 0; i < selectlist.Count(); i++)
//        {
//            Console.WriteLine(selectlist);
//        }
//    }

//}
