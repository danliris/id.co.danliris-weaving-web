using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class BeamProductionResumeDetailDailyOperationSizingCommand
    {
        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "BeamProductionResumeDate")]
        public DateTimeOffset BeamProductionResumeDate { get; set; }

        [JsonProperty(PropertyName = "BeamProductionResumeTime")]
        public TimeSpan BeamProductionResumeTime { get; set; }
    }

    public class ResumeBeamProductionDetailDailyOperationSizingCommandValidator : AbstractValidator<BeamProductionResumeDetailDailyOperationSizingCommand>
    {
        public ResumeBeamProductionDetailDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.BeamProductionResumeDate).NotEmpty();
            RuleFor(validator => validator.BeamProductionResumeTime).NotEmpty();
        }
    }
}
