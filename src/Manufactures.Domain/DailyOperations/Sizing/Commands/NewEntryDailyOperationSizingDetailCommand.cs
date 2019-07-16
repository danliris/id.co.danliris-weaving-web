using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class NewEntryDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }
    }

    public class NewEntryDailyOperationSizingDetailCommandValidator : AbstractValidator<NewEntryDailyOperationSizingDetailCommand>
    {
        public NewEntryDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftId.Value).NotEmpty();
            RuleFor(validator => validator.PreparationDate).NotEmpty();
            RuleFor(validator => validator.PreparationTime).NotEmpty();
        }
    }
}
