using Manufactures.Domain.DailyOperations;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperationalMachine
{
    public class DailyOperationalMachineListDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        public DailyOperationalMachineListDto(DailyOperationalMachineDocument document, string machineNumber)
        {
            Id = document.Identity;
            DateOperated = document.DateOperated;
            MachineNumber = machineNumber;
            UnitId = document.UnitId;
        }
    }
}
