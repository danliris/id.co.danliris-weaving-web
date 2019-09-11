using Manufactures.Domain.Beams;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping.DTOs
{
    public class DailyOperationBeamProduct
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; private set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; private set; }

        [JsonProperty(PropertyName = "Length")]
        public double Length { get; private set; }

        [JsonProperty(PropertyName = "Tention")]
        public int Tention { get; private set; }

        [JsonProperty(PropertyName = "Speed")]
        public int Speed { get; private set; }

        [JsonProperty(PropertyName = "PressRoll")]
        public double PressRoll { get; private set; }

        [JsonProperty(PropertyName = "BeamStatus")]
        public string BeamStatus { get; private set; }

        public DailyOperationBeamProduct(DailyOperationWarpingBeamProduct beamProduct, BeamDocument beamDocument)
        {
            Id = beamProduct.Identity;
            BeamId = new BeamId(beamProduct.BeamId);
            BeamNumber = beamDocument.Number;
            Length = beamProduct.Length ?? 0;
            Tention = beamProduct.Tention ?? 0;
            Speed = beamProduct.Speed ?? 0;
            PressRoll = beamProduct.PressRoll ?? 0;
            BeamStatus = beamProduct.BeamStatus;
        }
    }
}
