using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Warp = Manufactures.Domain.Construction.ValueObjects.Warp;
using Weft = Manufactures.Domain.Construction.ValueObjects.Weft;

namespace Manufactures.Domain.Construction.Commands
{
    public class UpdateConstructionCommand : ICommand<ConstructionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; set; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; set; }

        [JsonProperty(PropertyName = "Width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "WovenType")]
        public string WovenType { get; set; }

        [JsonProperty(PropertyName = "WarpTypeForm")]
        public string WarpTypeForm { get; set; }

        [JsonProperty(PropertyName = "WeftTypeForm")]
        public string WeftTypeForm { get; set; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; set; }

        [JsonProperty(PropertyName = "MaterialTypeDocument")]
        public MaterialTypeValueObject MaterialTypeDocument { get; set; }

        [JsonProperty(PropertyName = "ItemsWarp")]
        public List<Warp> ItemsWarp { get; set; }

        [JsonProperty(PropertyName = "ItemsWeft")]
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
