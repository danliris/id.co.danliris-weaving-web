using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class FabricConstructionDocument : ValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        public FabricConstructionDocument(Guid id, 
                                          string constructionNumber)
        {
            Id = id;
            ConstructionNumber = constructionNumber;
        }
        
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return ConstructionNumber;
        }
    }
}
