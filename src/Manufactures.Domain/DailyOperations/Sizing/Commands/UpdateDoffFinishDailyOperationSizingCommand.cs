using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public SizingCounterCommand Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public SizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public double TexSQ { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public double Visco { get; set; }

        [JsonProperty(PropertyName = "PISM")]
        public int PISM { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        [JsonProperty(PropertyName = "SizingBeamDocumentId")]
        public BeamId SizingBeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public UpdateDoffFinishDailyOperationSizingDetailCommand Details { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
    public class UpdateDoffFinishDailyOperationSizingCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingCommand>
    {
        public UpdateDoffFinishDailyOperationSizingCommandValidator()
        {
            //RuleFor(command => command.Counter).SetValidator(new SizingCounterCommandValidator());
            //RuleFor(command => command.Weight).SetValidator(new SizingWeightCommandValidator());
            RuleFor(validator => validator.MachineSpeed).NotEmpty();
            RuleFor(validator => validator.TexSQ).NotEmpty();
            RuleFor(validator => validator.Visco).NotEmpty();
            RuleFor(validator => validator.PISM).NotEmpty();
            RuleFor(validator => validator.SPU).NotEmpty();
            RuleFor(validator => validator.SizingBeamDocumentId).NotEmpty();
        }
    }
}
