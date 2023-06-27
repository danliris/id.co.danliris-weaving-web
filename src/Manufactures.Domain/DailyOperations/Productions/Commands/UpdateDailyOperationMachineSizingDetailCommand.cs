using FluentValidation;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Productions.Commands
{
    public class UpdateDailyOperationMachineSizingDetailCommand : AddNewDailyOperationMachineSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateDailyOperationMachineSizingDetailCommandValidator : AbstractValidator<UpdateDailyOperationMachineSizingDetailCommand>
    {
        public UpdateDailyOperationMachineSizingDetailCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
