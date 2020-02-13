using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingBeamsWarpingDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamNumber")]
        public string WarpingBeamNumber { get; set; }

        [JsonProperty(PropertyName = "WarpingYarnStrands")]
        public double WarpingYarnStrands { get; set; }

        public DailyOperationSizingBeamsWarpingDto(DailyOperationSizingBeamsWarping beamsWarping, string warpingBeamNumber)
        {
            Id = beamsWarping.Identity;
            WarpingBeamNumber = warpingBeamNumber;
            WarpingYarnStrands = beamsWarping.YarnStrands;
        }
    }
}
