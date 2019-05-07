using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingCausesCommand
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public string BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public string MachineTroubled { get; set; }
    }
}
