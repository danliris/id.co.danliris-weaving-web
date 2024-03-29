﻿using Newtonsoft.Json;
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
        public double AmountOfWarp { get; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public double AmountOfWeft { get; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; }

        public ConstructionDocumentValueObject(Guid identity,
                                               string constructionNumber,
                                               double amountOfWarp,
                                               double amountOfWeft,
                                               double totalYarn)
        {
            Identity = identity;
            ConstructionNumber = constructionNumber;
            AmountOfWarp = amountOfWarp;
            AmountOfWeft = amountOfWeft;
            TotalYarn = totalYarn;
        }
    }
}
