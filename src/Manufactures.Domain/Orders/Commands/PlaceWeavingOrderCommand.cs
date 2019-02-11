using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class PlaceWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        public FabricConstructionDocument FabricConstructionDocument { get; set; }
        public DateTimeOffset DateOrdered { get; set; }
        public string WarpOrigin { get; set; }
        public string WeftOrigin { get; set; }
        public int WholeGrade { get; set; }
        public string YarnType { get; set; }
        public Period Period { get; set; }
        public Composition Composition { get; set; }
        public WeavingUnit WeavingUnit { get; set; }
        public string OrderStatus { get; set; }
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
            RuleFor(command => command.Composition.CompositionOfPoly).NotEmpty();
            RuleFor(command => command.Composition.CompositionOfCotton).NotEmpty();
            RuleFor(command => command.Composition.OtherComposition).NotEmpty();
            RuleFor(command => command.WeavingUnit._id).NotEmpty();
            RuleFor(command => command.WeavingUnit.Name).NotEmpty();
        }
    }
}
