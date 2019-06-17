using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingBeamsCollectionValueObject : ValueObject
    {
        public Guid Id { get; set; }
        
        public string Number { get; set; }
        
        public string Type { get; set; }
        
        public double EmptyWeight { get; set; }

        public DailyOperationSizingBeamsCollectionValueObject(Guid id, string number, string type, double emptyWeight)
        {
            Id = id;
            Number = number;
            Type = type;
            EmptyWeight = emptyWeight;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return Number;
            yield return Type;
            yield return EmptyWeight;
        }
    }
}
