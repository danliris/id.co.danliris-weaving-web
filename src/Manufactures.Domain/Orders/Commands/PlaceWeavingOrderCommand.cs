using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class PlaceWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public FabricConstructionCommand FabricConstructionDocument { get; set; }

        [JsonProperty(PropertyName = "DateOrdered")]
        public DateTimeOffset DateOrdered { get; set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; set; }

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

    public class WeavingOrderCommandValidator : AbstractValidator<PlaceWeavingOrderCommand>
    {
        public WeavingOrderCommandValidator()
        {
            RuleFor(command => command.FabricConstructionDocument.Id).NotEmpty();
            RuleFor(command => command.FabricConstructionDocument.ConstructionNumber).NotEmpty();
            RuleFor(command => command.DateOrdered).NotEmpty();
            RuleFor(command => command.WarpOrigin).NotEmpty();
            RuleFor(command => command.WeftOrigin).NotEmpty();
            RuleFor(command => command.WholeGrade).NotEmpty();
            RuleFor(command => command.YarnType).NotEmpty();
            RuleFor(command => command.Period.Month).NotEmpty();
            RuleFor(command => command.Period.Year).NotEmpty();
            RuleFor(command => command.WeavingUnit.Id).NotEmpty();
        }
    }
}
