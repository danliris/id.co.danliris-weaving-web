using FluentValidation;
using Infrastructure.Domain.Commands;
//using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Productions.Commands
{
    public class UpdateDailyOperationMachineSizingCommand : ICommand<DailyOperationMachineSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<UpdateDailyOperationMachineSizingDetailCommand> EstimatedDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateDailyOperationMachineSizingCommandValidator : AbstractValidator<UpdateDailyOperationMachineSizingCommand>
    {
        public UpdateDailyOperationMachineSizingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.EstimatedDetails).NotEmpty();
        }
    }
}
