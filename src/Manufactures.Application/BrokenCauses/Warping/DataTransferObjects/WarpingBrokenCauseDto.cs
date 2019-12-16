using Manufactures.Domain.BrokenCauses.Warping;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.BrokenCauses.Warping.DataTransferObjects
{
    public class WarpingBrokenCauseDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(propertyName: "WarpingBrokenCauseName")]
        public string WarpingBrokenCauseName { get; }

        [JsonProperty(propertyName: "Information")]
        public string Information { get; }

        [JsonProperty(propertyName: "WarpingBrokenCauseCategory")]
        public string WarpingBrokenCauseCategory { get; }

        public WarpingBrokenCauseDto(WarpingBrokenCauseDocument document,
                                     string warpingBrokenCauseCategory)
        {
            Id = document.Identity;
            WarpingBrokenCauseName = document.WarpingBrokenCauseName;
            Information = document.Information;
            WarpingBrokenCauseCategory = warpingBrokenCauseCategory;
        }

        public WarpingBrokenCauseDto()
        {
        }
    }
}
