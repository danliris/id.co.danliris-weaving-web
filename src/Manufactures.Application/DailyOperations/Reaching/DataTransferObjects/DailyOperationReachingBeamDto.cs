using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingBeamDto
    {
        [JsonProperty(PropertyName = "ReachingBeamId")]
        public Guid ReachingBeamId { get; set; }

        [JsonProperty(PropertyName = "ReachingBeamNumber")]
        public string ReachingBeamNumber { get; set; }

        [JsonProperty(PropertyName = "CombNumber")]
        public int CombNumber { get; set; }

        public DailyOperationReachingBeamDto(Guid reachingBeamId,
                                             string reachingBeamNumber, 
                                             int combNumber)
        {
            ReachingBeamId = reachingBeamId;
            ReachingBeamNumber = reachingBeamNumber;
            CombNumber = combNumber;
        }
    }
}
