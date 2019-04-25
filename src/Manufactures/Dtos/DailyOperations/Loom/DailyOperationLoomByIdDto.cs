using Manufactures.Domain.DailyOperations.Loom;
using Manufactures.Domain.DailyOperations.Loom.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.Machine;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Dtos.DailyOperations.Loom
{
    public class DailyOperationLoomByIdDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public MachineDocumentDto MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "DailyOperationalMachineDetails")]
        public List<DailyOperationLoomDetailsValueObject> DailyOperationalMachineDetails { get; set; }

        public DailyOperationLoomByIdDto(DailyOperationLoomDocument document, MachineDocumentDto machineNumber, OrderDocumentValueObject order)
        {
            Id = document.Identity;
            DateOperated = document.DateOperated;
            MachineNumber = machineNumber;
            UnitId = document.UnitId;
            Status = document.Status;
            DailyOperationalMachineDetails = new List<DailyOperationLoomDetailsValueObject>();
        }
    }
}
