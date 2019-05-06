using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; set; }

        [JsonProperty(PropertyName = "ProductionTime")]
        public DailyOperationSizingProductionTimeCommand ProductionTime { get; set; }

        [JsonProperty(PropertyName = "BeamTime")]
        public DailyOperationSizingBeamTimeCommand BeamTime { get; set; }

        [JsonProperty(PropertyName = "BrokenBeam")]
        public int BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "TroubledMachine")]
        public int TroubledMachine { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public double Counter { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }
    }

    public class DailyOperationSizingDetailCommandValidator : AbstractValidator<DailyOperationSizingDetailCommand>
    {
        public DailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.BeamDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
