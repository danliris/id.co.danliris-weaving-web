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

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public double WeightNetto { get; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public double WeightBruto { get; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; }

        [JsonProperty(PropertyName = "TotalBroken")]
        public int TotalBroken { get; }

        [JsonProperty(PropertyName = "SizingBeamStatus")]
        public string SizingBeamStatus { get; }

        public DailyOperationSizingBeamProductDto(DailyOperationSizingBeamProduct beamProduct, string sizingBeamNumber)
        {
            Id = beamProduct.Identity;
            SizingBeamNumber = sizingBeamNumber;
            LatestDateTimeBeamProduct = beamProduct.LatestDateTimeBeamProduct;
            CounterStart = beamProduct.CounterStart;
            CounterFinish = beamProduct.CounterFinish;
            PISMeter = beamProduct.PISMeter;
            WeightNetto = beamProduct.WeightNetto;
            WeightBruto = beamProduct.WeightBruto;
            SPU = beamProduct.SPU;
            TotalBroken = beamProduct.TotalBroken;
            SizingBeamStatus = beamProduct.BeamStatus;
        }
    }
}
