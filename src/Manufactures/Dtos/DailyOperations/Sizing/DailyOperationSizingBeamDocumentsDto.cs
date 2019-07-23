using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentsDto
    {

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        [JsonProperty(PropertyName = "DateTimeBeamDocument")]
        public DateTimeOffset DateTimeBeamDocument { get; }

        //[JsonProperty(PropertyName = "Counter")]
        //public DailyOperationSizingBeamDocumentsCounterDto Counter { get; }

        [JsonProperty(PropertyName = "StartCounter")]
        public double StartCounter { get; }

        [JsonProperty(PropertyName = "FinishCounter")]
        public double FinishCounter { get; }

        //[JsonProperty(PropertyName = "Weight")]
        //public DailyOperationSizingBeamDocumentsWeightDto Weight { get; }

        [JsonProperty(PropertyName = "NettoWeight")]
        public double NettoWeight { get; }

        [JsonProperty(PropertyName = "BrutoWeight")]
        public double BrutoWeight { get; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; }

        public DailyOperationSizingBeamDocumentsDto(string sizingBeamNumber, DateTimeOffset dateTimeBeamDocumentHistory, double startCounter, double finishCounter, double nettoWeight, double brutoWeight, double pisMeter, double spu, string sizingBeamStatus)
        {
            SizingBeamNumber = sizingBeamNumber;
            DateTimeBeamDocument = dateTimeBeamDocumentHistory;
            StartCounter = startCounter;
            FinishCounter = finishCounter;
            NettoWeight = NettoWeight;
            BrutoWeight = brutoWeight;
            PISMeter = pisMeter;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
    }
}
