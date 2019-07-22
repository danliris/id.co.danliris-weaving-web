using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamCollectionsDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        //[JsonProperty(PropertyName = "Number")]
        //public string Number { get; }

        //[JsonProperty(PropertyName = "Type")]
        //public string Type { get; }

        //[JsonProperty(PropertyName = "EmptyWeight")]
        //public double EmptyWeight { get; }

        //[JsonProperty(PropertyName = "YarnLength")]
        //public double YarnLength { get; private set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; set; }

        public DailyOperationSizingBeamCollectionsDto(Guid id, double yarnStrands)
        {
            Id = id;
            YarnStrands = yarnStrands;
        }
    }
}
