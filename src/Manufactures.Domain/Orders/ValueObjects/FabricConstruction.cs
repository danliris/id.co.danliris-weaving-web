using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class FabricConstruction : ValueObject
    {
        public FabricConstruction(Guid id, string constructionNumber)
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
