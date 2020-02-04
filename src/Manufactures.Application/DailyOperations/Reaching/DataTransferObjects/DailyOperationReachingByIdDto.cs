using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingByIdDto : DailyOperationReachingListDto
    {
        [JsonProperty(PropertyName = "ReachingHistories")]
        public List<DailyOperationReachingHistoryDto> ReachingHistories { get; set; }
        [JsonProperty(PropertyName = "SizingYarnStrands")]
        public double SizingYarnStrands { get; set; }

        public DailyOperationReachingByIdDto(DailyOperationReachingDocument document, 
                                             DailyOperationReachingHistory history, 
                                             string machineNumber, 
                                             UnitId weavingUnitDocumentId, 
                                             string constructionNumber, 
                                             string sizingBeamNumber, 
                                             double sizingYarnStrands) 
            : base(document,
                   history,
                   machineNumber,
                   weavingUnitDocumentId,
                   constructionNumber,
                   sizingBeamNumber)
        {
            ReachingHistories = new List<DailyOperationReachingHistoryDto>();
            SizingYarnStrands = sizingYarnStrands;
        }
    }
}
