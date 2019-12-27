using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.DataTransferObjects.Movements
{
    public class BeamMovementDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; private set; }

        [JsonProperty(PropertyName = "BeamMovementDetails")]
        public List<BeamMovementDetailDto> BeamMovementDetails { get; set; }

        public BeamMovementDto(Guid movementDocumentId,
                          string movementStatus,
                          string beamNumber)
        {
            Id = movementDocumentId;
            Status = movementStatus;
            BeamNumber = beamNumber;
            BeamMovementDetails = new List<BeamMovementDetailDto>();
        }
    }
}
