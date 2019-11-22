using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomByIdDto : DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "DailyOperationLoomBeamHistories")]
        public List<DailyOperationLoomBeamHistoryDto> DailyOperationLoomBeamHistories { get; set; }

        public DailyOperationLoomByIdDto(DailyOperationLoomDocument document) : base(document)
        {
            DailyOperationLoomBeamHistories = new List<DailyOperationLoomBeamHistoryDto>();
        }

        public void AddDailyOperationLoomBeamHistories(DailyOperationLoomBeamHistoryDto history)
        {
            DailyOperationLoomBeamHistories.Add(history);
        }
    }
}
