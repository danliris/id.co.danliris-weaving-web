using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentsCounterDto
    {
        [JsonProperty(PropertyName = "Start")]
        public double Start { get; }

        [JsonProperty(PropertyName = "Finish")]
        public double Finish { get; }

        public DailyOperationSizingBeamDocumentsCounterDto(double start, double finish)
        {
            Start = start;
            Finish = finish;
        }
    }
}
