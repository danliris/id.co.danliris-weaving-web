using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class UpdateEstimationProductCommand : ICommand<EstimatedProductionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<UpdateEstimatedProductionDetailCommand> EstimatedDetails { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateEstimationProductCommandValidator : AbstractValidator<UpdateEstimationProductCommand>
    {
        public UpdateEstimationProductCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.EstimatedDetails).NotEmpty();
        }
    }
}
