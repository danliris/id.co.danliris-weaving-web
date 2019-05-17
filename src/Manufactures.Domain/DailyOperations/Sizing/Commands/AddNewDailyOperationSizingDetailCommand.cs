using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class AddNewDailyOperationSizingDetailCommand
    {

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "History")]
        public DailyOperationSizingHistoryCommand History { get; set; }
    }

    public class AddNewDailyOperationSizingDetailCommandValidator : AbstractValidator<AddNewDailyOperationSizingDetailCommand>
    {
        public AddNewDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.OperatorDocumentId.Value).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.History).SetValidator(new DailyOperationSizingHistoryCommandValidator());
        }
    }
}
