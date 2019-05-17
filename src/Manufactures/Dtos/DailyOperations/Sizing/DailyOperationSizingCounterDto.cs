using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingCounterDto
    {
        [JsonProperty(PropertyName = "Start")]
        public string Start { get; }

        [JsonProperty(PropertyName = "Finish")]
        public string Finish { get; }
    }
}
