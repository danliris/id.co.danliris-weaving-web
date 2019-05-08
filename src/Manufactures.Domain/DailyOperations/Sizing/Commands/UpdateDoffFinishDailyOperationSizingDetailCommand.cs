using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int? MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public double? TexSQ { get; set; }

        [JsonProperty(PropertyName = "ProductionTime")]
        public DailyOperationSizingProductionTimeCommand ProductionTime { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public double? Visco { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterCommand Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "PIS")]
        public int? PIS { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double? SPU { get; set; }

        [JsonProperty(PropertyName = "SizingBeamDocumentId")]
        public BeamId SizingBeamDocumentId { get; set; }
    }
    public class UpdateDoffDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingDetailCommand>
    {
        public UpdateDoffDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.MachineSpeed).NotEmpty();
            RuleFor(validator => validator.TexSQ).NotEmpty();
            RuleFor(validator => validator.ProductionTime.DoffFinish).NotEmpty();
            RuleFor(validator => validator.Visco).NotEmpty();
            RuleFor(validator => validator.Counter.Finish).NotEmpty();
            RuleFor(validator => validator.Weight.Bruto).NotEmpty();
            RuleFor(validator => validator.PIS).NotEmpty();
            RuleFor(validator => validator.SPU).NotEmpty();
            RuleFor(validator => validator.SizingBeamDocumentId.Value).NotEmpty();
        }
    }
}
