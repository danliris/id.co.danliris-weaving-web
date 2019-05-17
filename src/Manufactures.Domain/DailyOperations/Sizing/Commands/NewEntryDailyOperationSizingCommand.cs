using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class NewEntryDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public UnitId WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "RecipeCode")]
        public string RecipeCode { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterCommand Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamCollectionDocumentId")]
        public List<BeamId> WarpingBeamCollectionDocumentId { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public NewEntryDailyOperationSizingDetailCommand Details { get; set; }
    }

    public class NewEntryDailyOperationSizingCommandValidator : AbstractValidator<NewEntryDailyOperationSizingCommand>
    {
        public NewEntryDailyOperationSizingCommandValidator()
        {
            RuleFor(command => command.MachineDocumentId.Value).NotEmpty();
            RuleFor(command => command.WeavingUnitId.Value).NotEmpty();
            RuleFor(command => command.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(command => command.Counter).SetValidator(new DailyOperationSizingCounterCommandValidator());
            RuleFor(command => command.Weight).SetValidator(new DailyOperationSizingWeightCommandValidator());
            RuleFor(command => command.WarpingBeamCollectionDocumentId.Count).NotEqual(0);
            RuleFor(command => command.Details).SetValidator(new NewEntryDailyOperationSizingDetailCommandValidator());
        }
    }
}
