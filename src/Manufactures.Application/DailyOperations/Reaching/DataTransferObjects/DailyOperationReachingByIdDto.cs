using Manufactures.Domain.DailyOperations.Reaching;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingByIdDto : DailyOperationReachingListDto
    {
        [JsonProperty(PropertyName = "DailyOperationDetails")]
        public List<DailyOperationReachingDetailDto> DailyOperationDetails { get; private set; }

        public void SetDailyOperationDetails(DailyOperationReachingDetailDto value)
        {
            DailyOperationDetails.Add(value);
        }
    }
}
