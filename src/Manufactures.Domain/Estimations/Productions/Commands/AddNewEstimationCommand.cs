using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class AddNewEstimationCommand : ICommand<EstimatedProductionDocument>
    {
        [JsonProperty(PropertyName = "Period")]
        public DateTime Period { get; set; }

        [JsonProperty(PropertyName = "Unit")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimatedProductionDetail> EstimationProducts { get; set; }
    }

    public class AddNewEstimationCommandValidator : AbstractValidator<AddNewEstimationCommand>
    {
        public AddNewEstimationCommandValidator()
        {
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.UnitId).NotEmpty();
            RuleFor(command => command.EstimationProducts).NotEmpty();
        }
    }
}
