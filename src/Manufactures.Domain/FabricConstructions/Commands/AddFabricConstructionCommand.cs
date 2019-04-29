using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions.ValueObjects;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class AddFabricConstructionCommand : ICommand<FabricConstructionDocument>
    {
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

        [JsonProperty(PropertyName = "MaterialTypeName")]
        public string MaterialTypeName { get; set; }

        [JsonProperty(PropertyName = "ItemsWarp")]
        public List<ConstructionDetail> ItemsWarp { get; set; }

        [JsonProperty(PropertyName = "ItemsWeft")]
        public List<ConstructionDetail> ItemsWeft { get; set; }
    }

    public class PlaceConstructionCommandValidator : AbstractValidator<AddFabricConstructionCommand>
    {
        public PlaceConstructionCommandValidator()
        {
            RuleFor(command => command.ConstructionNumber).NotEmpty();
            RuleFor(command => command.AmountOfWarp).NotEmpty();
            RuleFor(command => command.AmountOfWeft).NotEmpty();
            RuleFor(command => command.Width).NotEmpty();
            RuleFor(command => command.WarpTypeForm).NotEmpty();
            RuleFor(command => command.WeftTypeForm).NotEmpty();
            RuleFor(command => command.TotalYarn).NotEmpty();
            RuleFor(command => command.MaterialTypeName).NotEmpty();
        }
    }
}
