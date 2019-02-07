using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.ValueObjects;
using Manufactures.Domain.Yarns.ValueObjects;
using System;
using System.Collections.Generic;
using Warp = Manufactures.Domain.Construction.ValueObjects.Warp;
using Weft = Manufactures.Domain.Construction.ValueObjects.Weft;

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
        public MaterialTypeDocument MaterialType { get; set; }
        public List<Warp> ItemsWarp { get; set; }
        public List<Weft> ItemsWeft { get; set; }
    }

    public class UpdateConstructionCommandValidator : AbstractValidator<UpdateConstructionCommand>
    {
        public UpdateConstructionCommandValidator()
        {
            RuleFor(command => command.ConstructionNumber).NotEmpty();
            RuleFor(command => command.AmountOfWarp).NotEmpty();
            RuleFor(command => command.AmountOfWeft).NotEmpty();
            RuleFor(command => command.Width).NotEmpty();
            RuleFor(command => command.WovenType).NotEmpty();
            RuleFor(command => command.WarpType).NotEmpty();
            RuleFor(command => command.WeftType).NotEmpty();
            RuleFor(command => command.TotalYarn).NotEmpty();
            RuleFor(command => command.MaterialType).NotEmpty();
            RuleFor(command => command.ItemsWarp).NotEmpty();
            RuleFor(command => command.ItemsWeft).NotEmpty();
        }
    }
}
