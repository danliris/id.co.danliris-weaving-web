using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Entities;
using Manufactures.Domain.Construction.ValueObjects;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Construction.Commands
{
    public class UpdateConstructionCommand : ICommand<ConstructionDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
        public string ConstructionNumber { get; set; }
        public int AmountOfWarp { get; set; }
        public int AmountOfWeft { get; set; }
        public int Width { get; set; }
        public string WovenType { get; set; }
        public string WarpType { get; set; }
        public string WeftType { get; set; }
        public double TotalYarn { get; set; }
        public MaterialType MaterialType { get; set; }
        public List<ConstructionDetail> Warps { get; set; }
        public List<ConstructionDetail> Wefts { get; set; }
    }

    public class UpdateConstructionCommandValidator : AbstractValidator<UpdateConstructionCommand>
    {
        public UpdateConstructionCommandValidator()
        {
            RuleFor(command => command.ConstructionNumber).NotNull();
            RuleFor(command => command.AmountOfWarp).NotNull();
            RuleFor(command => command.AmountOfWeft).NotNull();
            RuleFor(command => command.Width).NotNull();
            RuleFor(command => command.WovenType).NotNull();
            RuleFor(command => command.WarpType).NotNull();
            RuleFor(command => command.WeftType).NotNull();
            RuleFor(command => command.TotalYarn).NotNull();
            RuleFor(command => command.MaterialType).NotNull();
            RuleFor(command => command.Warps).NotNull();
            RuleFor(command => command.Wefts).NotNull();
        }
    }
}
