using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class UpdateWeavingOrderCommand : ICommand<WeavingOrderDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public FabricConstructionDocument FabricConstructionDocument { get; set; }
        public string WarpOrigin { get; set; }
        public string WeftOrigin { get; set; }
        public int WholeGrade { get; set; }
        public string YarnType { get; set; }
        public Period Period { get; set; }
        public Composition WarpComposition { get; set; }
        public Composition WeftComposition { get; set; }
        public WeavingUnit WeavingUnit { get; set; }
        public string OrderStatus { get; set; }
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
