using FluentValidation;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class UpdateEstimatedProductionDetailCommand : AddNewEstimatedProductionDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateEstimatedProductionDetailCommandValidator : AbstractValidator<UpdateEstimatedProductionDetailCommand>
    {
        public UpdateEstimatedProductionDetailCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
        }
    }
}
