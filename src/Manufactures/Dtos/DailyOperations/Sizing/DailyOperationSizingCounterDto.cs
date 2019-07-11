using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingCounterDto
    {
        [JsonProperty(PropertyName = "Start")]
        public double Start { get; }

        [JsonProperty(PropertyName = "Finish")]
        public double Finish { get; }

        public DailyOperationSizingCounterDto(double start, double finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}
