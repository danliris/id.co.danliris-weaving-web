using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingByIdDto : DailyOperationReachingListDto
    {
        public DailyOperationReachingByIdDto(DailyOperationReachingDocument document, DailyOperationReachingDetail detail, string machineNumber, UnitId weavingUnitDocumentId, string constructionNumber, string sizingBeamNumber) 
            : base(document,
                   detail,
                   machineNumber,
                   document.WeavingUnitId,
                   constructionNumber,
                   sizingBeamNumber)
        {
        }
    }
}
