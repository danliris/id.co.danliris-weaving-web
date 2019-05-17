using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingHistoryDto
    {
        [JsonProperty(PropertyName = "TimeOnMachine")]
        public DateTimeOffset TimeOnMachine { get;  }

        [JsonProperty(PropertyName = "MachineStatus")]
        public string MachineStatus { get; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get;  }
    }
}
