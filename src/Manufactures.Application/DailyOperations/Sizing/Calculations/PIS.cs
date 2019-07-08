using Manufactures.Domain.Shared.Calculations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Calculation
{
    public class PIS
    {
        public int Calculate(int cutmarkInput)
        {
            var result = 0;

            if (cutmarkInput != 0)
            {
                result = cutmarkInput * ConstantCalculations.CUTMARK_STANDARD_VALUE;
            }

            return result;
        }
    }
}
