using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingWeightDto
    {
        [JsonProperty(PropertyName = "Netto")]
        public string Netto { get; }

        [JsonProperty(PropertyName = "Bruto")]
        public string Bruto { get; }
    }
}
