using Manufactures.Domain.Shared.Calculations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Calculation
{
    public class PIS
    {
        public double CalculateInMeter(double counterStart, double counterFinish)
        {
            double pisInMeter = 0;

            if (counterStart >= 0 && counterFinish != 0)
            {
                pisInMeter = (counterFinish - counterStart);
            }

            return pisInMeter;
        }

        public double CalculateInPieces(double counterStart, double counterFinish)
        {
            double pisInMeter = 0;
            double pisInPieces = 0;

            if (counterStart >= 0 && counterFinish != 0)
            {
                pisInMeter = counterFinish - counterStart;
                pisInPieces = pisInMeter / ConstantsValue.CUTMARK_STANDARD_VALUE;
            }

            return pisInPieces;
        }
    }
}
