using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdatePauseDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationSizingDetails")]
        public UpdatePauseDailyOperationSizingDetailCommand UpdatePauseDailyOperationSizingDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdatePauseDailyOperationSizingCommandValidator
       : AbstractValidator<UpdatePauseDailyOperationSizingCommand>
    {
        public UpdatePauseDailyOperationSizingCommandValidator()
        {
            RuleFor(command => command.UpdatePauseDailyOperationSizingDetails).SetValidator(new UpdatePauseDailyOperationSizingDetailCommandValidator());
        }
    }
}
