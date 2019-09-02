using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.Calculations
{
    public class Netto
    {
        public double CalculateNetto(double emptyWeight, double bruto)
        {
            double result = 0;

            if (emptyWeight != 0 && bruto != 0)
            {
                result = bruto - emptyWeight;
            }

            return result;
        }
    }
}
