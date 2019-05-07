using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingWeightValueObject : ValueObject
    {
        public string Netto { get; set; }
        
        public string Bruto { get; set; }

        public DailyOperationSizingWeightValueObject(string netto, string bruto)
        {
            Netto = netto;
            Bruto = bruto;
        }

        public DailyOperationSizingWeightValueObject(DailyOperationSizingWeightCommand dailyOperationSizingWeightProduction)
        {
            Netto = dailyOperationSizingWeightProduction.Netto;
            Bruto = dailyOperationSizingWeightProduction.Bruto;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Netto;
            yield return Bruto;
        }
    }
}
