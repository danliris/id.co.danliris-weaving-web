using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentsDto
    {
        [JsonProperty(PropertyName = "DateTimeBeamDocumentHistory")]
        public DateTimeOffset DateTimeBeamDocumentHistory { get; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingBeamDocumentsCounterDto Counter { get; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingBeamDocumentsWeightDto Weight { get; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; }

        public DailyOperationSizingBeamDocumentsDto(DateTimeOffset dateTimeBeamDocumentHistory, DailyOperationSizingBeamDocumentsCounterDto counter, DailyOperationSizingBeamDocumentsWeightDto weight, double pisMeter, double spu, string sizingBeamStatus)
        {
            DateTimeBeamDocumentHistory = dateTimeBeamDocumentHistory;
            Counter = counter;
            Weight = weight;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
    }
}
