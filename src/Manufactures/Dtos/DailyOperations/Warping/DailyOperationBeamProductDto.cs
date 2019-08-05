using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Application.DailyOperations.Warping
{
    public class DailyOperationBeamProductDto
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; private set; }

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

        public DailyOperationBeamProductDto(DailyOperationWarpingBeamProduct x)
        {
            Id = x.Identity;
            BeamId = new BeamId(x.BeamId);
            Length = x.Length ?? 0;
            Tention = x.Tention ?? 0;
            Speed = x.Speed ?? 0;
            PressRoll = x.PressRoll ?? 0;
            BeamStatus = x.BeamStatus;
        }
    }
}
