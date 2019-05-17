using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryCommand History { get; set; }
    }
    public class UpdateDoffDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingDetailCommand>
    {
        public UpdateDoffDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.History).SetValidator(new DailyOperationSizingHistoryCommandValidator());
        }
    }
}
