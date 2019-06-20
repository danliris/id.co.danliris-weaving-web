using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.Movements
{
    public class BeamMovementListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; private set; }

        public BeamMovementListDto(Guid movementDocumentId,
                           string movementStatus,
                           string beamNumber)
        {
            Id = movementDocumentId;
            Status = movementStatus;
            BeamNumber = beamNumber;
        }
    }
}
