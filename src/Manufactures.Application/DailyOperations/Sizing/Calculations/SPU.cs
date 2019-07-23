namespace Manufactures.Application.DailyOperations.Sizing.Calculations
{
    public class SPU
    {
        public double Calculate(double netto, double theoritical)
        {
            double result = 0;

            if (netto != 0 && theoritical != 0)
            {
                result = (netto - theoritical) * 100 / theoritical;
            }

            return result;
        }
    }
}
