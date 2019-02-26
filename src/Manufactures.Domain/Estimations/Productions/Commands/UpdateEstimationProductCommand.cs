using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class UpdateEstimationProductCommand : ICommand<EstimatedProductionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; set; }

        [JsonProperty(PropertyName = "Unit")]
        public WeavingUnit Unit { get; set; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimationProductValueObject> EstimationProducts { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateEstimationProductCommandValidator : AbstractValidator<UpdateEstimationProductCommand>
    {
        public UpdateEstimationProductCommandValidator()
        {
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.Unit).NotEmpty();
        }
    }
}
