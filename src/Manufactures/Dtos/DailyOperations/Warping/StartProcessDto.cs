using Manufactures.Application.DailyOperations.Warping;
using Manufactures.Application.DailyOperations.Warping.DTOs;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Dtos.DailyOperations.Warping
{
    public class StartProcessDto
    {
        [JsonProperty(PropertyName = "DailyOperationBeamProductDtos")]
        public List<DailyOperationBeamProductDto> DailyOperationBeamProductDtos { get; }

        [JsonProperty(PropertyName = "DailyOperationHistories")]
        public List<DailyOperationHistory> DailyOperationHistories { get; }

        public StartProcessDto(List<DailyOperationBeamProductDto> dailyOperationBeamProductDtos,
                               List<DailyOperationHistory> dailyOperationHistories)
        {
            DailyOperationBeamProductDtos = dailyOperationBeamProductDtos;
            DailyOperationHistories = dailyOperationHistories;
        }
    }
}
