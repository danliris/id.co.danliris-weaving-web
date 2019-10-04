using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class PlaceOrderCommand : ICommand<OrderDocument>
    {
        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public FabricConstructionCommand FabricConstructionDocument { get; set; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; set; }

        [JsonProperty(PropertyName = "WarpOriginId")]
        public string WarpOriginId { get; set; }

        [JsonProperty(PropertyName = "WeftOriginId")]
        public string WeftOriginId { get; set; }

        [JsonProperty(PropertyName = "WholeGrade")]
        public int WholeGrade { get; set; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; set; }

        [JsonProperty(PropertyName = "Period")]
        public Period Period { get; set; }

        [JsonProperty(PropertyName = "WarpComposition")]
        public Composition WarpComposition { get; set; }

        [JsonProperty(PropertyName = "WeftComposition")]
        public Composition WeftComposition { get; set; }

        [JsonProperty(PropertyName = "WeavingUnit")]
        public WeavingUnit WeavingUnit { get; set; }
    }

    public class WeavingOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public WeavingOrderCommandValidator()
        {
            RuleFor(command => command.FabricConstructionDocument.Id).NotEmpty();
            RuleFor(command => command.FabricConstructionDocument.ConstructionNumber).NotEmpty();
            RuleFor(command => command.DateOrdered).NotEmpty();
            RuleFor(command => command.WarpOriginId).NotEmpty();
            RuleFor(command => command.WeftOriginId).NotEmpty();
            RuleFor(command => command.WholeGrade).NotEmpty();
            RuleFor(command => command.YarnType).NotEmpty();
            RuleFor(command => command.Period.Month).NotEmpty();
            RuleFor(command => command.Period.Year).NotEmpty();
            RuleFor(command => command.WeavingUnit.Id).NotEmpty();
        }
    }
}
