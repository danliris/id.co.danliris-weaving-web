using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperationalMachine
{
    public class DailyOperationalMachineListDto
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

        public DailyOperationalMachineListDto(DailyOperationalMachineDocument document,
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
