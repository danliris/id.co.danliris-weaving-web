using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions.Entity;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class AddFabricConstructionCommand : ICommand<FabricConstructionDocument>
    {
        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; private set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; private set; }

        [JsonProperty(PropertyName = "WovenType")]
        public string WovenType { get; private set; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; private set; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; private set; }

        [JsonProperty(PropertyName = "Width")]
        public int Width { get; private set; }

        [JsonProperty(PropertyName = "WarpType")]
        public string WarpType { get; private set; }

        [JsonProperty(PropertyName = "WeftType")]
        public string WeftType { get; private set; }

        [JsonProperty(PropertyName = "ReedSpace")]
        public double ReedSpace { get; private set; }

        [JsonProperty(PropertyName = "YarnStrandsAmount")]
        public double YarnStrandsAmount { get; private set; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; private set; }

        [JsonProperty(PropertyName = "ConstructionWarpsDetail")]
        public List<ConstructionYarnDetailCommand> ConstructionWarpsDetail { get; private set; }

        [JsonProperty(PropertyName = "ConstructionWeftsDetail")]
        public List<ConstructionYarnDetailCommand> ConstructionWeftsDetail { get; private set; }
    }

    public class PlaceConstructionCommandValidator : AbstractValidator<AddFabricConstructionCommand>
    {
        public PlaceConstructionCommandValidator()
        {
            RuleFor(command => command.ConstructionNumber).NotEmpty().WithMessage("No. Konstruksi Tidak Boleh Kosong");
            RuleFor(command => command.MaterialType).NotEmpty().WithMessage("Jenis Material Harus Diisi");
            RuleFor(command => command.WovenType).NotEmpty().WithMessage("Jenis Anyaman Harus Diisi");
            RuleFor(command => command.AmountOfWarp).NotEmpty().WithMessage("Jumlah Lusi Harus Diisi");
            RuleFor(command => command.AmountOfWeft).NotEmpty().WithMessage("Jumlah Pakan Harus Diisi");
            RuleFor(command => command.Width).NotEmpty().WithMessage("Lebar Harus Diisi");
            RuleFor(command => command.WarpType).NotEmpty().WithMessage("Tipe Lusi Tidak Boleh Kosong");
            RuleFor(command => command.WeftType).NotEmpty().WithMessage("Tipe Pakan Tidak Boleh Kosong");
            RuleFor(command => command.ReedSpace).NotEmpty().WithMessage("Reed Space Harus Diisi");
            RuleFor(command => command.YarnStrandsAmount).NotEmpty().WithMessage("Jumlah Helai Benang Harus Diisi");
            RuleFor(command => command.TotalYarn).NotEmpty().WithMessage("Total Benang Tidak Boleh Kosong");
            RuleFor(command => command.ConstructionWarpsDetail).NotEmpty().WithMessage("Lusi Harus Diisi");
            RuleFor(command => command.ConstructionWeftsDetail).NotEmpty().WithMessage("Pakan Harus Diisi");
        }
    }
}
