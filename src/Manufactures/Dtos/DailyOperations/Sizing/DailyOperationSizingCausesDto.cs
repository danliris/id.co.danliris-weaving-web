using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingCausesDto
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public string BrokenBeam { get; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public string MachineTroubled { get; }
    }
}
