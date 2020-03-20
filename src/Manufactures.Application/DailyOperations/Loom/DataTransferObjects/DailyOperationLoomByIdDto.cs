using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomByIdDto : DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "DailyOperationLoomBeamsUsed")]
        public List<DailyOperationLoomBeamUsedDto> DailyOperationLoomBeamsUsed { get; set; }

        [JsonProperty(PropertyName = "DailyOperationLoomBeamHistories")]
        public List<DailyOperationLoomHistoryDto> DailyOperationLoomBeamHistories { get; set; }

        public DailyOperationLoomByIdDto(DailyOperationLoomDocument document) : base(document)
        {
            DailyOperationLoomBeamsUsed = new List<DailyOperationLoomBeamUsedDto>();
            DailyOperationLoomBeamHistories = new List<DailyOperationLoomHistoryDto>();
        }

        public void AddDailyOperationLoomBeamProducts(DailyOperationLoomBeamUsedDto beamProduct)
        {
            DailyOperationLoomBeamsUsed.Add(beamProduct);
        }

        public void AddDailyOperationLoomBeamHistories(DailyOperationLoomHistoryDto history)
        {
            DailyOperationLoomBeamHistories.Add(history);
        }
    }
}
