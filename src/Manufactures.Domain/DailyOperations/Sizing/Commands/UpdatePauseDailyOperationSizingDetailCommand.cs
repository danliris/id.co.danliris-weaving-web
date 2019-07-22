using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdatePauseDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "PauseDate")]
        public DateTimeOffset PauseDate { get; set; }

        [JsonProperty(PropertyName = "PauseTime")]
        public TimeSpan PauseTime { get; set; }

        [JsonProperty(PropertyName = "Causes")]
        public DailyOperationSizingCauseCommand Causes { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }
    }

    public class UpdatePauseDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdatePauseDailyOperationSizingDetailCommand>
    {
        public UpdatePauseDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.PauseDate).NotEmpty();
            RuleFor(validator => validator.PauseTime).NotEmpty();
            RuleFor(validator => validator.Causes).SetValidator(new DailyOperationSizingCausesCommandValidator());
        }
    }
}
