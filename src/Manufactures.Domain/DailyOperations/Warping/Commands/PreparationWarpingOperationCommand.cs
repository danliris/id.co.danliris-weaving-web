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
        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "MaterialTypeId")]
        public MaterialTypeId MaterialTypeId { get; set; }

        [JsonProperty(PropertyName = "AmountOfCones")]
        public int AmountOfCones { get; set; }

        [JsonProperty(PropertyName = "ColourOfCone")]
        public string ColourOfCone { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }
    }

    public class PreparationWarpingOperationCommandValidator 
        : AbstractValidator<PreparationWarpingOperationCommand>
    {
        public PreparationWarpingOperationCommandValidator()
        {
            RuleFor(command => command.OrderDocumentId.Value).NotEmpty();
            RuleFor(command => command.MaterialTypeId.Value).NotEmpty();
            RuleFor(command => command.AmountOfCones).NotEmpty();
            RuleFor(command => command.ColourOfCone).NotEmpty();
            RuleFor(command => command.OperatorDocumentId.Value).NotEmpty();
            RuleFor(command => command.PreparationDate).NotEmpty();
            RuleFor(command => command.PreparationTime).NotEmpty();
            RuleFor(command => command.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
