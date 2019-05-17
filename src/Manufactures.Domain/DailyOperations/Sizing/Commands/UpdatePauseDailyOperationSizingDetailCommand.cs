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
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryCommand History { get; set; }

        [JsonProperty(PropertyName = "Causes")]
        public DailyOperationSizingCausesCommand Causes { get; set; }
    }

    public class UpdatePauseDailyOperationSizingDetailCommandValidator
       : AbstractValidator<UpdatePauseDailyOperationSizingDetailCommand>
    {
        public UpdatePauseDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.History).SetValidator(new DailyOperationSizingHistoryCommandValidator());
            RuleFor(command => command.Causes).SetValidator(new DailyOperationSizingCausesCommandValidator());
        }
    }
}
