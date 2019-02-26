using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class UpdateWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionDocument")]
        public FabricConstructionDocument FabricConstructionDocument { get; set; }

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

        [JsonProperty(PropertyName = "OrderStatus")]
        public string OrderStatus { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateWeavingOrderCommandValidator : AbstractValidator<UpdateWeavingOrderCommand>
    {
        public UpdateWeavingOrderCommandValidator()
        {
            RuleFor(command => command.FabricConstructionDocument.Id).NotEmpty();
            RuleFor(command => command.FabricConstructionDocument.ConstructionNumber).NotEmpty();
            RuleFor(command => command.WarpOrigin).NotEmpty();
            RuleFor(command => command.WeftOrigin).NotEmpty();
            RuleFor(command => command.WholeGrade).NotEmpty();
            RuleFor(command => command.YarnType).NotEmpty();
            RuleFor(command => command.Period.Month).NotEmpty();
            RuleFor(command => command.Period.Year).NotEmpty();
            RuleFor(command => command.WarpComposition.CompositionOfPoly).NotEmpty();
            RuleFor(command => command.WarpComposition.CompositionOfCotton).NotEmpty();
            RuleFor(command => command.WarpComposition.OtherComposition).NotEmpty();
            RuleFor(command => command.WeftComposition.CompositionOfPoly).NotEmpty();
            RuleFor(command => command.WeftComposition.CompositionOfCotton).NotEmpty();
            RuleFor(command => command.WeftComposition.OtherComposition).NotEmpty();
            RuleFor(command => command.WeavingUnit._id).NotEmpty();
            RuleFor(command => command.WeavingUnit.Name).NotEmpty();
        }
    }
}
