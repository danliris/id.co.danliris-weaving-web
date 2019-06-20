using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.Movements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.Movements
{
    public class BeamListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; private set; }

        public BeamListDto(Guid movementDocumentId,
                           string movementStatus,
                           string beamNumber)
        {
            Id = movementDocumentId;
            Status = movementStatus;
            BeamNumber = beamNumber;
        }
    }
}
