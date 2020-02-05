using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.DailyOperations.Reaching.DataTransferObjects
{
    public class DailyOperationReachingListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public string WeavingUnit { get; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        public DailyOperationReachingListDto(DailyOperationReachingDocument document, 
                                             DailyOperationReachingHistory history, 
                                             string machineNumber, 
                                             string weavingUnitDocumentId, 
                                             string constructionNumber, 
                                             string sizingBeamNumber)
        {
            Id = document.Identity;
            DateTimeOperation = history.DateTimeMachine;
            MachineNumber = machineNumber;
            WeavingUnit = weavingUnitDocumentId;
            ConstructionNumber = constructionNumber;
            SizingBeamNumber = sizingBeamNumber;
        }
    }
}
