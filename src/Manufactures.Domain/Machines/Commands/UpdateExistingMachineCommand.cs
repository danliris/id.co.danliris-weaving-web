using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Machines.Commands
{
    public class UpdateExistingMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "MachineTypeId")]
        public MachineTypeId MachineTypeId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public UnitId WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "Cutmark")]
        public int? Cutmark { get; set; }

        [JsonProperty(PropertyName = "CutmarkUom")]
        public string CutmarkUom { get; set; }

        [JsonProperty(PropertyName = "Process")]
        public string Process { get; set; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "Block")]
        public int Block { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateExistingMachineCommandValidator : AbstractValidator<UpdateExistingMachineCommand>
    {
        public UpdateExistingMachineCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty().WithMessage("Id Tidak Boleh Kosong");
            RuleFor(r => r.MachineNumber).NotEmpty().WithMessage("No. Mesin Harus Diisi");
            RuleFor(r => r.Location).NotEmpty().WithMessage("Lokasi Harus Diisi");
            RuleFor(r => r.MachineTypeId).NotEmpty().WithMessage("Tipe Mesin Harus Diisi");
            RuleFor(r => r.WeavingUnitId).NotEmpty().WithMessage("Unit Weaving Harus Diisi");
            RuleFor(r => r.Process).NotEmpty().WithMessage("Proses Harus Diisi");
            RuleFor(r => r.Area).NotEmpty().WithMessage("Area Harus Diisi");
            RuleFor(r => r.Block).NotNull().WithMessage("Blok Harus Diisi");
        }
    }
}
