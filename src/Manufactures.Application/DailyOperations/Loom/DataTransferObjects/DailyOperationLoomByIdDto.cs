using Manufactures.Domain.DailyOperations.Loom;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Loom.DataTransferObjects
{
    public class DailyOperationLoomByIdDto : DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "DailyOperationLoomBeamProducts")]
        public List<DailyOperationLoomBeamProductDto> DailyOperationLoomBeamProducts { get; set; }

        [JsonProperty(PropertyName = "DailyOperationLoomBeamHistories")]
        public List<DailyOperationLoomBeamHistoryDto> DailyOperationLoomBeamHistories { get; set; }

        public DailyOperationLoomByIdDto(DailyOperationLoomDocument document) : base(document)
        {
            DailyOperationLoomBeamProducts = new List<DailyOperationLoomBeamProductDto>();
            DailyOperationLoomBeamHistories = new List<DailyOperationLoomBeamHistoryDto>();
        }

        public void AddDailyOperationLoomBeamProducts(DailyOperationLoomBeamProductDto beamProduct)
        {
            DailyOperationLoomBeamProducts.Add(beamProduct);
        }

        public void AddDailyOperationLoomBeamHistories(DailyOperationLoomBeamHistoryDto history)
        {
            DailyOperationLoomBeamHistories.Add(history);
        }
    }
}
