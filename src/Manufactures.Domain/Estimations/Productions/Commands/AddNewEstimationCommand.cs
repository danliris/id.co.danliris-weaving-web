using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class AddNewEstimationCommand : ICommand<EstimatedProductionDocument>
    {
        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; set; }

        [JsonProperty(PropertyName = "Unit")]
        public WeavingUnit Unit { get; set; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimationProductValueObject> EstimationProducts { get; set; }
    }

    public class AddNewEstimationCommandValidator : AbstractValidator<AddNewEstimationCommand>
    {
        public AddNewEstimationCommandValidator()
        {
            RuleFor(command => command.Period).NotEmpty();
            RuleFor(command => command.Unit).NotEmpty();
        }
    }
}
