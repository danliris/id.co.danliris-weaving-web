using FluentValidation;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Productions.Commands
{
    public class AddNewDailyOperationMachineSizingDetailCommand
    {
        [JsonProperty(PropertyName = "OrderId")]
        public Guid OrderId { get; private set; }

        [JsonProperty(PropertyName = "GradeA")]
        public double GradeA { get; private set; }

        [JsonProperty(PropertyName = "GradeB")]
        public double GradeB { get; private set; }

        [JsonProperty(PropertyName = "GradeC")]
        public double GradeC { get; private set; }

        [JsonProperty(PropertyName = "GradeD")]
        public double GradeD { get; private set; }
    }

    public class AddNewDailyOperationMachineSizingDetailCommandValidator : AbstractValidator<AddNewDailyOperationMachineSizingDetailCommand>
    {
        public AddNewDailyOperationMachineSizingDetailCommandValidator()
        {
            RuleFor(command => command.OrderId).NotEmpty();
            RuleFor(command => command.GradeA).NotEmpty();
            RuleFor(command => command.GradeB).NotEmpty();
            RuleFor(command => command.GradeC).NotEmpty();
        }
    }
}
