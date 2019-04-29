using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingBeamTimeCommand
    {
        [JsonProperty(PropertyName = "Install")]
        public DateTimeOffset Install { get; set; }

        [JsonProperty(PropertyName = "Uninstall")]
        public DateTimeOffset Uninstall { get; set; }
    }
}
