using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class PlaceWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        public string OrderNumber { get; set; }
        public FabricConstructionDocument FabricConstructionDocument { get; set; }
        public DateTimeOffset DateOrdered { get; set; }
        public string WarpOrigin { get; set; }
        public string WeftOrigin { get; set; }
        public int WholeGrade { get; set; }
        public string YarnType { get; set; }
        public Period Period { get; set; }
        public Composition Composition { get; set; }
        public WeavingUnit WeavingUnit { get; set; }
    }

    public class WeavingOrderCommandValidator : AbstractValidator<PlaceWeavingOrderCommand>
    {
        public WeavingOrderCommandValidator()
        {
            RuleFor(command => command.OrderNumber).NotNull();
            RuleFor(command => command.FabricConstructionDocument).NotNull();
            RuleFor(command => command.DateOrdered).NotNull();
            RuleFor(command => command.WarpOrigin).NotNull();
            RuleFor(command => command.WeftOrigin).NotNull();
            RuleFor(command => command.WholeGrade).NotNull();
            RuleFor(command => command.YarnType).NotNull();
            RuleFor(command => command.Period).NotNull();
            RuleFor(command => command.Composition).NotNull();
            RuleFor(command => command.WeavingUnit).NotNull();
        }
    }
}
