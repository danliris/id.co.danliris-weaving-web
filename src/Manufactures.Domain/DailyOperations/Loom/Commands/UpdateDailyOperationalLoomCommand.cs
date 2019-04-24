using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdateDailyOperationalLoomCommand : ICommand<DailyOperationalLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; private set; }

        [JsonProperty(PropertyName = "MachineId")]
        public MachineId MachineId { get; set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "Status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "DailyOperationMachineDetails")]
        public List<DailyOperationLoomDetailCommand> DailyOperationMachineDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
