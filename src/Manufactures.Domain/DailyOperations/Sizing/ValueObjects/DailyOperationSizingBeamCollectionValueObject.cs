using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.ValueObjects
{
    public class DailyOperationSizingBeamsCollectionValueObject : ValueObject
    {
        public Guid Id { get; set; }
        
        //public string Number { get; set; }
        
        //public string Type { get; set; }

        public double YarnStrands { get; set; }

        public DailyOperationSizingBeamsCollectionValueObject(Guid id, double yarnStrands)
        {
            Id = id;
            //Number = number;
            //Type = type;
            YarnStrands = yarnStrands;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            //yield return Number;
            //yield return Type;
            yield return YarnStrands;
        }
    }
}
