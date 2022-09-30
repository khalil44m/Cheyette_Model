using Calibration;
using ClassLibrary1;
using MathNet.Numerics.LinearAlgebra;
using ModelFactory;
using CsvHelper;

namespace Solution
{
    enum OutPut
        {
            ATMvol,
            Smile
        }

    public class Solution{
        public static void Main(string[] args){
            
            
            double beta = 1.0;
            double[,] correlation = new[,] {{1.0, 0.7, 0.5, 0}, {0.7, 1.0, 0.8, 0}, {0.5, 0.8, 1.0, 0}, {0, 0, 0, 1.0}};
            double[] lambdas = new[] {0.3, 0.06, 0};
            double nu = 0.01;
            double gamma = 0.3;
            int factornb = 3;
            var maturitylist = new List<double> { 1 / 12.0, 1 / 6.0, 0.25, 0.5, 0.75, 1, 2, 3, 4, 5, 7, 10 };
            int maturitynb = 6;
            double horizon = 20.0;
            double pricingstep = 1/252.0;
            double calibrationstep = 0.01;
            double localtenor = 10.0;
            double deterministictenor = 5.0;
            double a = 1.0;
            double b = 0.0001;
            int pathNumber = 32000;
            int particleNumber = 10000;

            var useMode = Calibration.UseMode.LSD;
            var output = OutPut.Smile;

            ModelParameters parameters = new ModelParameters(beta, ConvertToMatrix.Convert(correlation), lambdas, nu,
                                                            gamma, factornb, maturitylist[maturitynb], 0.0, horizon, pricingstep,
                                                             calibrationstep, localtenor, deterministictenor, a, b);
            CalibParameters parameters_ = new CalibParameters(particleNumber, pathNumber);

            //////LOCAL/////////////////////////////////////////////////////////////////////////////////////
            MarketDataFactory.Init(parameters);
            var simulationtrajectories = SimulationTrajectoriesFactory.BuildTrajectories(useMode, parameters, parameters_);
            var localpricer = new Pricer(useMode, simulationtrajectories, localtenor);
            var localimpliedvolmodel = new ImpliedVolatilityModel(localpricer);
            var continuousIVMarket = MarketDataFactory.BuildImpliedVolatilityMarket(localtenor).ContinuousIVM;

            var strikeList = MarketDataFactory.BuildImpliedVolatilityData()[$"{localtenor}"]
                                                            .Item2[maturitylist.IndexOf(maturitylist[6])];

            Matrix<double> excel = Matrix<double>.Build.Dense(5, strikeList.Count);

            /*for (int k = 0; k < strikeList.Count; k++)
            {
                excel[0, k] = strikeList[k];
                excel[1, k] = localpricer.Price(strikeList[k]);
                excel[2, k] = BachelierVanillaCalculator.Price(maturity, strikeList[k], continuousIVMarket.Eval(maturity, strikeList[k]), localtenor);
                excel[3, k] = localimpliedvolmodel.Eval(maturity, strikeList[k]);
                excel[4, k] = continuousIVMarket.Eval(maturity, strikeList[k]);
            }
            WriteExcel.Writedata(useMode == UseMode.LSD ?
                $"Model-Market {useMode} {maturity}y-{localtenor}y-D{deterministictenor}" :
                $"Model-Market {useMode} {maturity}y-{localtenor}y", excel);*/

            using(var w = new StreamWriter("./Output.csv"))
            {
                var header = string.Format("Strike, Model Price, Market Price, Model vol, Market Vol");
                w.WriteLine(header);
                w.Flush();

                for (int k = 0; k < strikeList.Count; k++)
                {
                    var strike = strikeList[k];
                    var modelprice = localpricer.Price(strikeList[k]);
                    var marketprice = BachelierVanillaCalculator.Price(maturitylist[maturitynb], strikeList[k],
                                 continuousIVMarket.Eval(maturitylist[maturitynb], strikeList[k]), localtenor);
                    var modelvol = localimpliedvolmodel.Eval(maturitylist[maturitynb], strikeList[k]);
                    var impliedvol = continuousIVMarket.Eval(maturitylist[maturitynb], strikeList[k]);
            
                    var line = string.Format("{0},{1},{2},{3},{4}", strike, modelprice, marketprice, modelvol, impliedvol);
                    w.WriteLine(line);
                    w.Flush();
                }
            }

            //////DETERMINISTIC/////////////////////////////////////////////////////////////////////////////////////
            /*useMode = Calibration.UseMode.LSD;

            var simulationtrajectories = SimulationTrajectoriesFactory.BuildTrajectories(useMode, pathNumber, particleNumber);
            var deterministicpricer = new Pricer(useMode, simulationtrajectories, deterministictenor);
            var maturitylist = new List<double> { 1 / 12.0, 1 / 6.0, 0.25, 0.5, 0.75, 1, 2, 3, 4, 5, 7, 10};

            var strikeATM = MarketDataFactory.BuildRatesMarket().Instrument(deterministictenor).InitialSwapFunction.Eval(maturity);
            var IVmarket = MarketDataFactory.BuildImpliedVolatilityData()[$"{deterministictenor}"].Item1[maturitylist.IndexOf(maturity)][40]; //40 == moneyness 0, ATM
            var price = deterministicpricer.Price(strikeATM);

            Console.WriteLine(price);
            Console.WriteLine(BachelierVanillaCalculator.PriceAtm(maturity, IVmarket, deterministictenor));
            Console.WriteLine(price * Math.Sqrt( 2 * Math.PI / maturity) / MarketDataFactory.BuildRatesMarket().Instrument(deterministictenor).InitialBPV.Eval(maturity));
            Console.WriteLine(IVmarket);*/

    }
}
}
