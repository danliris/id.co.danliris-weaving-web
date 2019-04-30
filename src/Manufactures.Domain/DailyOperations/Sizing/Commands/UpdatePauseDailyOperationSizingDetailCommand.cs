using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdatePauseDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "BrokenBeam")]
        public int BrokenBeam { get; private set; }

        [JsonProperty(PropertyName = "TroubledMachine")]
        public int TroubledMachine { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }
    }
}
