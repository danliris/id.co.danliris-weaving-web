using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Commands
{
    public class AddNewDailyOperationCommand : ICommand<DailyOperationMachineDocument>
    {
        [JsonProperty(PropertyName = "MachineId")]
        public MachineId MachineId { get; set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "DailyOperationMachineDetails")]
        public List<DailyOperationsValueObject> DailyOperationMachineDetails { get; set; }
    }
}
