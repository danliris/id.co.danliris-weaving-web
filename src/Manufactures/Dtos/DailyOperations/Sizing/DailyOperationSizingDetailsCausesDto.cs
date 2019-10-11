using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingDetailsCausesDto
    {
        [JsonProperty(PropertyName = "BrokenBeam")]
        public int BrokenBeam { get; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public int MachineTroubled { get; }

        public DailyOperationSizingDetailsCausesDto(int brokenBeam, int machineTroubled)
        {
            BrokenBeam = brokenBeam;
            MachineTroubled = machineTroubled;
        }
    }
}
