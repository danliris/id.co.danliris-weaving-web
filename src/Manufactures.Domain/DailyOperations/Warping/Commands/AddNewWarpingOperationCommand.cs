using System;
using System.Collections.Generic;
using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class AddNewWarpingOperationCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        public ConstructionId ConstructionId { get; set; }
        public MaterialTypeId MaterialTypeId { get; set; }
        public int AmountOfCones { get; set; }
        public string ColourOfCone { get; set; }
        public DateTimeOffset DateOperation { get; set; }
        public String TimeOperation { get; set; }
        public OperatorId OperatorId { get; set; }
    }

    public class AddNewWarpingOperationCommandValidator 
        : AbstractValidator<AddNewWarpingOperationCommand>
    {
        public AddNewWarpingOperationCommandValidator()
        {
            RuleFor(command => command.ConstructionId.Value).NotEmpty();
            RuleFor(command => command.MaterialTypeId.Value).NotEmpty();
            RuleFor(command => command.AmountOfCones).NotEmpty();
            RuleFor(command => command.ColourOfCone).NotEmpty();
            RuleFor(command => command.DateOperation).NotEmpty();
            RuleFor(command => command.TimeOperation).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
        }
    }
}
