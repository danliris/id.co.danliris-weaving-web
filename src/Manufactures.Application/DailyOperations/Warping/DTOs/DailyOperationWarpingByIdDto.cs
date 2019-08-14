using Manufactures.Domain.DailyOperations.Warping;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationWarpingByIdDto : DailyOperationWarpingListDto
    {
        [JsonProperty(PropertyName = "DailyOperationBeamProducts")]
        public List<DailyOperationBeamProduct> DailyOperationBeamProducts { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationLoomHistories")]
        public List<DailyOperationLoomHistoryDto> DailyOperationLoomHistories { get; private set; }

        public DailyOperationWarpingByIdDto(DailyOperationWarpingDocument document)
            : base(document)
        {
            DailyOperationBeamProducts = new List<DailyOperationBeamProduct>();
            DailyOperationLoomHistories = new List<DailyOperationLoomHistoryDto>();
        }

        public void AddDailyOperationBeamProducts(DailyOperationBeamProduct value)
        {
            if (!DailyOperationBeamProducts.Any(product => product.Id.Equals(value.Id)))
            {
                DailyOperationBeamProducts.Add(value);
            }
        }

        public void AddDailyOperationLoomHistories(DailyOperationLoomHistoryDto value)
        {
            DailyOperationLoomHistories.Add(value);
        }
    }
}
