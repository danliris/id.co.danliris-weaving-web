using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ResumeBeamProductionDetailDailyOperationSizingCommand
    {
        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "ResumeBeamProductionDate")]
        public DateTimeOffset ResumeBeamProductionDate { get; set; }

        [JsonProperty(PropertyName = "ResumeBeamProductionTime")]
        public TimeSpan ResumeBeamProductionTime { get; set; }
    }

    public class ResumeBeamProductionDetailDailyOperationSizingCommandValidator : AbstractValidator<ResumeBeamProductionDetailDailyOperationSizingCommand>
    {
        public ResumeBeamProductionDetailDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.ResumeBeamProductionDate).NotEmpty();
            RuleFor(validator => validator.ResumeBeamProductionDate).NotEmpty();
        }
    }
}
