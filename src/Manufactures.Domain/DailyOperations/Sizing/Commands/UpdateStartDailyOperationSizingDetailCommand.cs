using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryCommand History { get; set; }
    }

    public class UpdateStartDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateStartDailyOperationSizingDetailCommand>
    {
        public UpdateStartDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.ShiftDocumentId.Value).NotEmpty();
            RuleFor(command => command.History).SetValidator(new DailyOperationSizingHistoryCommandValidator());
        }
    }
}
