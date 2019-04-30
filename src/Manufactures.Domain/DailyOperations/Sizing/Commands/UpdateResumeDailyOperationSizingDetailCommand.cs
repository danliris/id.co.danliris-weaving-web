using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateResumeDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        //[JsonProperty(PropertyName = "ConstructionDocumentId")]
        //public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "BeamTime")]
        public DailyOperationSizingBeamTimeCommand BeamTime { get; set; }
    }

    public class UpdateResumeDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateResumeDailyOperationSizingDetailCommand>
    {
        public UpdateResumeDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.BeamDocumentId.Value).NotEmpty();
            //RuleFor(validator => validator.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.BeamTime).SetValidator(new DailyOperationSizingBeamTimeCommandValidator());
        }
    }
}
