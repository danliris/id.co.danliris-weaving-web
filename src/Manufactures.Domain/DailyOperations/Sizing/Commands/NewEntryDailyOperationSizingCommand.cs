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

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamsId")]
        public List<BeamId> WarpingBeamsId { get; set; }

        [JsonProperty(PropertyName = "SizingBeamDocuments")]
        public NewEntryDailyOperationSizingBeamDocumentCommand SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public NewEntryDailyOperationSizingDetailCommand Details { get; set; }
    }

    public class NewEntryDailyOperationSizingCommandValidator : AbstractValidator<NewEntryDailyOperationSizingCommand>
    {
        public NewEntryDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.MachineDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.WeavingUnitId.Value).NotEmpty();
            RuleFor(validator => validator.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.RecipeCode).NotEmpty();
            RuleFor(validator => validator.NeReal).NotEmpty();
            RuleFor(validator => validator.WarpingBeamsId.Count).NotEqual(0);
            RuleFor(validator => validator.SizingBeamDocuments).SetValidator(new DailyOperationSizingBeamDocumentCommandValidator()); ;
            RuleFor(validator => validator.Details).SetValidator(new NewEntryDailyOperationSizingDetailCommandValidator());
        }
    }
}
