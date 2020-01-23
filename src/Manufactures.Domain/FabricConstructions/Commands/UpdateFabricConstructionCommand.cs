using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class UpdateFabricConstructionCommand : ICommand<FabricConstructionDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "ConstructionNumber")]
        public string ConstructionNumber { get; set; }

        [JsonProperty(PropertyName = "MaterialType")]
        public string MaterialType { get; set; }

        [JsonProperty(PropertyName = "WovenType")]
        public string WovenType { get; set; }

        [JsonProperty(PropertyName = "AmountOfWarp")]
        public int AmountOfWarp { get; set; }

        [JsonProperty(PropertyName = "AmountOfWeft")]
        public int AmountOfWeft { get; set; }

        [JsonProperty(PropertyName = "Width")]
        public int Width { get; set; }

        [JsonProperty(PropertyName = "WarpType")]
        public string WarpType { get; set; }

        [JsonProperty(PropertyName = "WeftType")]
        public string WeftType { get; set; }

        [JsonProperty(PropertyName = "ReedSpace")]
        public int ReedSpace { get; private set; }

        [JsonProperty(PropertyName = "YarnStrandsAmount")]
        public int YarnStrandsAmount { get; private set; }

        [JsonProperty(PropertyName = "TotalYarn")]
        public double TotalYarn { get; set; }

        [JsonProperty(PropertyName = "ConstructionWarpsDetail")]
        public List<ConstructionYarnDetailCommand> ConstructionWarpsDetail { get; set; }

        [JsonProperty(PropertyName = "ConstructionWeftsDetail")]
        public List<ConstructionYarnDetailCommand> ConstructionWeftsDetail { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateConstructionCommandValidator : AbstractValidator<UpdateFabricConstructionCommand>
    {
        public UpdateConstructionCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id Tidak Boleh Kosong");
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
