using Manufactures.Domain.DailyOperations.ReachingTying;
using Manufactures.Domain.DailyOperations.ReachingTying.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.ReachingTying.DataTransferObjects
{
    public class DailyOperationReachingTyingByIdDto : DailyOperationReachingTyingListDto
    {
        [JsonProperty(PropertyName = "ReachingHistories")]
        public List<DailyOperationReachingTyingDetailDto> ReachingHistories { get; set; }

        public DailyOperationReachingTyingByIdDto(DailyOperationReachingTyingDocument document, DailyOperationReachingTyingDetail detail, string machineNumber, UnitId weavingUnitDocumentId, string constructionNumber, string sizingBeamNumber) 
            : base(document,
                   detail,
                   machineNumber,
                   weavingUnitDocumentId,
                   constructionNumber,
                   sizingBeamNumber)
        {
            ReachingHistories = new List<DailyOperationReachingTyingDetailDto>();
        }
    }
}
