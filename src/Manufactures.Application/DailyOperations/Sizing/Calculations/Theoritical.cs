using Manufactures.Domain.Shared.Calculations;

namespace Manufactures.Application.DailyOperations.Sizing.Calculations
{
    public class Theoritical
    {
        public double CalculateKawamoto(double pisInMeter, double yarnStrands, double neReal)
        {
            double result = 0;

            if (pisInMeter != 0 && yarnStrands != 0 && neReal != 0)
            {
                result = pisInMeter * yarnStrands * ConstantsValue.NETTO_CONSTANT_VALUE / ConstantsValue.KAWAMOTO_CONSTANT_VALUE * neReal;
            }

            return result;
        }

        public double CalculateSuckerMuller(double pisInMeter, double yarnStrands, double neReal)
        {
            double result = 0;

            if (pisInMeter != 0 && yarnStrands != 0 && neReal != 0)
            {
                result = pisInMeter * yarnStrands * ConstantsValue.NETTO_CONSTANT_VALUE / ConstantsValue.SUCKERMULLER_CONSTANT_VALUE * neReal;
            }

            return result;
        }
    }
}
