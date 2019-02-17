using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class FabricConstructionDocument : ValueObject
    {
        public FabricConstructionDocument(Guid id, 
                                          string constructionNumber)
        {
            Id = id;
            ConstructionNumber = constructionNumber;
        }

        public Guid Id { get; private set; }
        public string ConstructionNumber { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ConstructionNumber;
        }
    }
}
