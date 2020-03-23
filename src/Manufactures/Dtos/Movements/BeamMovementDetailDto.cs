using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.DataTransferObjects.Movements
{
    public class BeamMovementDetailDto
    {
        //private DailyOperationLoomDetail detail;

        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateTimeMachine")]
        public DateTimeOffset DateTimeOperation { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        public BeamMovementDetailDto(DailyOperationSizingHistory detail)
        {
            Id = detail.Identity;
            DateTimeOperation = detail.DateTimeMachine;
            Information = detail.MachineStatus;
        }

        public BeamMovementDetailDto(DailyOperationLoomHistory detail)
        {
            Id = detail.Identity;
            DateTimeOperation = detail.DateTimeMachine;
            Information = detail.Information;
        }
    }
}
