using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ProduceBeamDetailDailyOperationSizingCommand
    {
        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamResumeDate")]
        public DateTimeOffset ProduceBeamResumeDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamResumeTime")]
        public TimeSpan ProduceBeamResumeTime { get; set; }
    }

    public class ProduceBeamDetailDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDetailDailyOperationSizingCommand>
    {
        public ProduceBeamDetailDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.ProduceBeamResumeDate).NotEmpty();
            RuleFor(validator => validator.ProduceBeamResumeTime).NotEmpty();
        }
    }
}
