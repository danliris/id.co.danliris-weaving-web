using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class ConstructionDocumentValueObject
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        public ConstructionDocumentValueObject(Guid identity,
                                               string constructionNumber,
                                               int amountOfWarp,
                                               int amountOfWeft,
                                               double totalYarn)
        {
            Identity = identity;
            ConstructionNumber = constructionNumber;
            TotalYarn = totalYarn;
        }
    }
}
