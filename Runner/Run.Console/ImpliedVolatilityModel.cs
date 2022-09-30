using ModelFactory;


namespace Calibration
{
    public class ImpliedVolatilityModel
    {
        private readonly Pricer pricer;

        public ImpliedVolatilityModel(Pricer pricer)
        {
            this.pricer = pricer;
        }
        public double Eval(double time, double strike)
        {
            var mcPrice = pricer.Price(strike);
            return BachelierVanillaCalculator.InversePrice(mcPrice, time, strike, pricer.Tenor);
        }
        /*public double EvalATM(double time)
        {
            return Eval(time, pricer.SimulateProcesses.ParticularCalibrator.Simulator.MarketData.RatesMarketD.InitialSwapRate.Eval(time));
        }*/
    }
}