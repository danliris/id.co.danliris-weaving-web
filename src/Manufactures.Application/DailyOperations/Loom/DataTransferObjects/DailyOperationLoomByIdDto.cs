using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomByIdDto : DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "BeamProcessed")]
        public int BeamProcessed { get; set; }

        [JsonProperty(PropertyName = "DailyOperationLoomBeamsUsed")]
        public List<DailyOperationLoomBeamsUsedDto> DailyOperationLoomBeamsUsed { get; set; }

        [JsonProperty(PropertyName = "DailyOperationLoomBeamHistories")]
        public List<DailyOperationLoomHistoryDto> DailyOperationLoomBeamHistories { get; set; }

        public DailyOperationLoomByIdDto(DailyOperationLoomDocument document, int beamProcessed) : base(document)
        {
            BeamProcessed = beamProcessed;
            DailyOperationLoomBeamsUsed = new List<DailyOperationLoomBeamsUsedDto>();
            DailyOperationLoomBeamHistories = new List<DailyOperationLoomHistoryDto>();
        }

        public void AddDailyOperationLoomBeamProducts(DailyOperationLoomBeamsUsedDto beamProduct)
        {
            DailyOperationLoomBeamsUsed.Add(beamProduct);
        }

        public void AddDailyOperationLoomBeamHistories(DailyOperationLoomHistoryDto history)
        {
            DailyOperationLoomBeamHistories.Add(history);
        }
    }
}
