using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Loom
{
    public class DailyOperationLoomListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; }

        [JsonProperty(PropertyName = "OrderNumber")]
        public string OrderNumber { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; }

        [JsonProperty(PropertyName = "StatusOperation")]
        public string Status { get; }

        public DailyOperationLoomListDto(DailyOperationLoomDocument document,
                                              string orderNumber,
                                              string machineNumber)
        {
            Id = document.Identity;
            DateOperated = document.DateOperated;
            OrderNumber = orderNumber;
            MachineNumber = machineNumber;
            UnitId = document.UnitId;
            Status = document.Status;
        }
    }
}
