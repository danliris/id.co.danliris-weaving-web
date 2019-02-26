using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using System;
using System.Collections.Generic;
using Warp = Manufactures.Domain.Construction.ValueObjects.Warp;
using Weft = Manufactures.Domain.Construction.ValueObjects.Weft;

namespace Manufactures.Domain.Construction.Commands
{
    public class UpdateConstructionCommand : ICommand<ConstructionDocument>
    {
        public Guid Id { get; private set; }
        public string ConstructionNumber { get; set; }
        public int AmountOfWarp { get; set; }
        public int AmountOfWeft { get; set; }
        public int Width { get; set; }
        public string WovenType { get; set; }
        public string WarpTypeForm { get; set; }
        public string WeftTypeForm { get; set; }
        public double TotalYarn { get; set; }
        public MaterialTypeValueObject MaterialTypeDocument { get; set; }
        public List<Warp> ItemsWarp { get; set; }
        public List<Weft> ItemsWeft { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
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
            RuleFor(command => command.WarpTypeForm).NotEmpty();
            RuleFor(command => command.WeftTypeForm).NotEmpty();
            RuleFor(command => command.TotalYarn).NotEmpty();

            RuleFor(command => command.MaterialTypeDocument.Id).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Code).NotEmpty();
            RuleFor(command => command.MaterialTypeDocument.Name).NotEmpty();

            RuleFor(command => command.ItemsWarp).NotEmpty();
            RuleFor(command => command.ItemsWeft).NotEmpty();
        }
    }
}
