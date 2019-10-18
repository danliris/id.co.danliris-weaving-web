using Manufactures.Domain.Beams;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Sizing.DataTransferObjects
{
    public class DailyOperationSizingBeamProductDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; }

        [JsonProperty(PropertyName = "LatestDateTimeBeamProduct")]
        public DateTimeOffset LatestDateTimeBeamProduct { get; }

        [JsonProperty(PropertyName = "CounterStart")]
        public double CounterStart { get; }

        [JsonProperty(PropertyName = "CounterFinish")]
        public double CounterFinish { get; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public double WeightNetto { get; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public double WeightBruto { get; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; }

        public DailyOperationSizingBeamProductDto(DailyOperationSizingBeamProduct beamProduct,
                                                  BeamDocument beamDocument)
        {
            Id = beamProduct.Identity;
            SizingBeamNumber = beamDocument.Number;
            LatestDateTimeBeamProduct = beamProduct.LatestDateTimeBeamProduct;
            CounterStart = beamProduct.CounterStart ?? 0;
            CounterFinish = beamProduct.CounterFinish ?? 0;
            WeightNetto = beamProduct.WeightNetto ?? 0;
            WeightBruto = beamProduct.WeightBruto ?? 0;
            PISMeter = beamProduct.PISMeter ?? 0;
            SPU = beamProduct.SPU ?? 0;
            SizingBeamStatus = beamProduct.BeamStatus;
        }
    }
}
