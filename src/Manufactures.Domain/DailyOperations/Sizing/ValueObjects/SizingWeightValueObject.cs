using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class SizingWeightValueObject : ValueObject
    {
        public double Netto { get; set; }

        public double Bruto { get; set; }

        public double Theoretical { get; set; }

        public SizingWeightValueObject(double netto, double bruto, double theoretical)
        {
            Netto = netto;
            Bruto = bruto;
            Theoretical = theoretical;
        }

        public SizingWeightValueObject()
        {
        }

        public SizingWeightValueObject(SizingWeightCommand dailyOperationSizingWeightProduction)
        {
            Netto = dailyOperationSizingWeightProduction.Netto;
            Bruto = dailyOperationSizingWeightProduction.Bruto;
            Theoretical = dailyOperationSizingWeightProduction.Theoretical;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Netto;
            yield return Bruto;
            yield return Theoretical;
        }
    }
}
