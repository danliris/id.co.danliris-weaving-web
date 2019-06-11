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

        [JsonProperty(PropertyName = "FinishDate")]
        public DateTimeOffset FinishDate { get; set; }

        [JsonProperty(PropertyName = "FinishTime")]
        public TimeSpan FinishTime { get; set; }
    }

    public class UpdateDoffDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingDetailCommand>
    {
        public UpdateDoffDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.FinishDate).NotEmpty();
            RuleFor(validator => validator.FinishTime).NotEmpty();
        }
    }
}
