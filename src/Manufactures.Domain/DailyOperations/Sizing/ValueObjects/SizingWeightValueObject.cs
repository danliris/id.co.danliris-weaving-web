using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class SizingWeightValueObject : ValueObject
    {
        public double Netto { get; set; }

        public double Bruto { get; set; }

        public double Theoritical { get; set; }

        public SizingWeightValueObject(double netto, double bruto, double theoritical)
        {
            Netto = netto;
            Bruto = bruto;
            Theoritical = theoritical;
        }

        public SizingWeightValueObject()
        {
        }

        public SizingWeightValueObject(SizingWeightCommand dailyOperationSizingWeightProduction)
        {
            Netto = dailyOperationSizingWeightProduction.Netto;
            Bruto = dailyOperationSizingWeightProduction.Bruto;
            Theoritical = dailyOperationSizingWeightProduction.Theoritical;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Netto;
            yield return Bruto;
            yield return Theoritical;
        }
    }
}
