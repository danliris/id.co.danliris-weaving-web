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

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateConstructionCommandValidator : AbstractValidator<UpdateFabricConstructionCommand>
    {
        public UpdateConstructionCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id Harus Valid");
            RuleFor(command => command.ConstructionNumber).NotNull().NotEmpty().WithMessage("No. Konstruksi Tidak Boleh Kosong");
            RuleFor(command => command.MaterialType).NotNull().NotEmpty().WithMessage("Jenis Material Harus Diisi");
            RuleFor(command => command.WovenType).NotNull().NotEmpty().WithMessage("Jenis Anyaman Harus Diisi");
            RuleFor(command => command.AmountOfWarp).NotNull().NotEmpty().WithMessage("Jumlah Lusi Harus Diisi");
            RuleFor(command => command.AmountOfWeft).NotNull().NotEmpty().WithMessage("Jumlah Pakan Harus Diisi");
            RuleFor(command => command.Width).NotNull().NotEmpty().WithMessage("Lebar Harus Diisi");
            RuleFor(command => command.WarpType).NotNull().NotEmpty().WithMessage("Tipe Lusi Tidak Boleh Kosong");
            RuleFor(command => command.WeftType).NotNull().NotEmpty().WithMessage("Tipe Pakan Tidak Boleh Kosong");
            RuleFor(command => command.ReedSpace).NotNull().NotEmpty().WithMessage("Reed Space Harus Diisi");
            RuleFor(command => command.YarnStrandsAmount).NotNull().NotEmpty().WithMessage("Jumlah Helai Benang Harus Diisi");
            RuleFor(command => command.TotalYarn).NotNull().NotEmpty().WithMessage("Total Benang Tidak Boleh Kosong");
            //RuleForEach(command => command.ConstructionWarpsDetail).SetValidator(new ConstructionYarnDetailCommandValidator()).WithMessage("Lusi Harus Diisi");
            //RuleForEach(command => command.ConstructionWeftsDetail).SetValidator(new ConstructionYarnDetailCommandValidator()).WithMessage("Pakan Harus Diisi");
        }
    }
}
