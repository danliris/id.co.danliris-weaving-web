using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationSizingDetails")]
        public UpdateDoffFinishDailyOperationSizingDetailCommand UpdateDoffFinishDailyOperationSizingDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
    public class UpdateDoffFinishDailyOperationSizingCommandValidator
     : AbstractValidator<UpdateDoffFinishDailyOperationSizingCommand>
    {
        public UpdateDoffFinishDailyOperationSizingCommandValidator()
        {
            RuleFor(command => command.UpdateDoffFinishDailyOperationSizingDetails).SetValidator(new UpdateDoffDailyOperationSizingDetailCommandValidator());
        }
    }
}
