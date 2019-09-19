using System;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class PreparationDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "PreparationOrder")]
        public OrderId PreparationOrder { get; set; }

        [JsonProperty(PropertyName = "PreparationMaterialType")]
        public MaterialTypeId PreparationMaterialType { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "ColourOfCone")]
        public string ColourOfCone { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "PreparationShift")]
        public ShiftId PreparationShift { get; set; }

        [JsonProperty(PropertyName = "PreparationOperator")]
        public OperatorId PreparationOperator { get; set; }
    }

    public class PreparationDailyOperationWarpingCommandValidator 
        : AbstractValidator<PreparationDailyOperationWarpingCommand>
    {
        public PreparationDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.PreparationOrder).NotEmpty();
            RuleFor(command => command.PreparationMaterialType).NotEmpty();
            RuleFor(command => command.AmountOfCones).NotEmpty();
            RuleFor(command => command.ColourOfCone).NotEmpty();
            RuleFor(command => command.PreparationDate).NotEmpty();
            RuleFor(command => command.PreparationTime).NotEmpty();
            RuleFor(command => command.PreparationShift).NotEmpty();
            RuleFor(command => command.PreparationOperator).NotEmpty();
        }
    }
}
