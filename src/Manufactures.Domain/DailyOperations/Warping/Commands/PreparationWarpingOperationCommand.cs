using System;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class PreparationWarpingOperationCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "OrderId")]
        public OrderId OrderId { get; set; }

        [JsonProperty(PropertyName = "ConstructionId")]
        public ConstructionId ConstructionId { get; set; }

        [JsonProperty(PropertyName = "MaterialTypeId")]
        public MaterialTypeId MaterialTypeId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "ColourOfCone")]
        public string ColourOfCone { get; set; }

        [JsonProperty(PropertyName = "DateOperation")]
        public DateTimeOffset DateOperation { get; set; }

        [JsonProperty(PropertyName = "TimeOperation")]
        public TimeSpan TimeOperation { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class PreparationWarpingOperationCommandValidator 
        : AbstractValidator<PreparationWarpingOperationCommand>
    {
        public PreparationWarpingOperationCommandValidator()
        {
            RuleFor(command => command.OrderId.Value).NotEmpty();
            RuleFor(command => command.ConstructionId.Value).NotEmpty();
            RuleFor(command => command.MaterialTypeId.Value).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.AmountOfCones).NotEmpty();
            RuleFor(command => command.ColourOfCone).NotEmpty();
            RuleFor(command => command.DateOperation).NotEmpty();
            RuleFor(command => command.TimeOperation).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
        }
    }
}
