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

        [JsonProperty(PropertyName = "ProduceBeamDate")]
        public DateTimeOffset ProduceBeamDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamTime")]
        public TimeSpan ProduceBeamTime { get; set; }
    }

    public class ProduceBeamDetailDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDetailDailyOperationSizingCommand>
    {
        public ProduceBeamDetailDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.ProduceBeamDate).NotEmpty();
            RuleFor(validator => validator.ProduceBeamTime).NotEmpty();
        }
    }
}
