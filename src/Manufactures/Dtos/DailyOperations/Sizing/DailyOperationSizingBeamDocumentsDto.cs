using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Dtos.DailyOperations.Sizing
{
    public class DailyOperationSizingBeamDocumentsDto
    {

        [JsonProperty(PropertyName = "DateTimeOperationBeamDocument")]
        public DateTimeOffset DateTimeOperationBeamDocument { get; }

        [JsonProperty(PropertyName = "ShiftName")]
        public string ShiftName { get; }

        [JsonProperty(PropertyName = "OperatorName")]
        public string OperatorName { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterDto Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingWeightDto Weight { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; private set; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; private set; }

        public DailyOperationSizingBeamDocumentsDto(DateTimeOffset dateTimeOperationBeamDocument, string shiftName, string operatorName, string sizingBeamNumber, DailyOperationSizingCounterDto counter, DailyOperationSizingWeightDto weight, double spu, string sizingBeamStatus)
        {
            DateTimeOperationBeamDocument = dateTimeOperationBeamDocument;
            ShiftName = shiftName;
            OperatorName = operatorName;
            SizingBeamNumber = sizingBeamNumber;
            Counter = counter;
            Weight = weight;
            SPU = spu;
            SizingBeamStatus = sizingBeamStatus;
        }
    }
}
