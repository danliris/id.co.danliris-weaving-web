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

        [JsonProperty(PropertyName = "Period")]
        public DateTime Period { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; private set; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimatedProductionDetail> EstimationProducts { get; set; }

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
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.UnitId).NotEmpty();
            RuleFor(command => command.EstimationProducts).NotEmpty();
        }
    }
}
