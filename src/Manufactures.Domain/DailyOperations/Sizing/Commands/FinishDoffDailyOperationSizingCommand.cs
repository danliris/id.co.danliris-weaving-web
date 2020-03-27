using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class FinishDoffDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "CounterFinish")]
        public double CounterFinish { get; set; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public double WeightNetto { get; set; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public double WeightBruto { get; set; }

        [JsonProperty(PropertyName = "WeightTheoritical")]
        public double WeightTheoritical { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamOperator")]
        public OperatorId ProduceBeamOperator { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamShift")]
        public ShiftId ProduceBeamShift { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamDate")]
        public DateTimeOffset ProduceBeamDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamTime")]
        public TimeSpan ProduceBeamTime { get; set; }

        [JsonProperty(PropertyName = "BrokenPerShift")]
        public int BrokenPerShift { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public int TexSQ { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public int Visco { get; set; }

        [JsonProperty(PropertyName = "IsFinishFlag")]
        public bool IsFinishFlag { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public string SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "SizingBeamNumber")]
        public string SizingBeamNumber { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
    public class FinishDoffDailyOperationSizingCommandValidator : AbstractValidator<FinishDoffDailyOperationSizingCommand>
    {
        public FinishDoffDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.CounterFinish).NotEmpty().WithMessage("Counter Akhir Harus Diisi");
            RuleFor(validator => validator.WeightNetto).NotEmpty().WithMessage("Netto Harus Diisi");
            RuleFor(validator => validator.WeightBruto).NotEmpty().WithMessage("Bruto Harus Diisi");
            RuleFor(validator => validator.WeightTheoritical).NotEmpty().WithMessage("Berat Teoritis Harus Diisi");
            RuleFor(validator => validator.PISMeter).NotEmpty().WithMessage("PIS Harus Diisi");
            RuleFor(validator => validator.SPU).NotEmpty().WithMessage("SPU Harus Diisi");
            RuleFor(validator => validator.ProduceBeamOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.ProduceBeamShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.ProduceBeamDate).NotEmpty().WithMessage("Tanggal Produksi Beam Harus Diisi");
            RuleFor(validator => validator.ProduceBeamTime).NotEmpty().WithMessage("Waktu Produksi Beam Harus Diisi");
            //RuleFor(validator => validator.BrokenPerShift).NotEmpty().WithMessage("Jumlah Putus Beam Harus Diisi");
            RuleFor(validator => validator.MachineSpeed).NotEmpty().WithMessage("Kecepatan Mesin Harus Diisi");
            RuleFor(validator => validator.TexSQ).NotEmpty().WithMessage("TexSQ Harus Diisi");
            RuleFor(validator => validator.Visco).NotEmpty().WithMessage("Visco Harus Diisi");
            RuleFor(validator => validator.SizingBeamId).NotEmpty().WithMessage("No. Beam Sizing Harus Diisi");
            RuleFor(validator => validator.SizingBeamNumber).NotEmpty().WithMessage("Number Beam Sizing Harus Diisi");
        }
    }
}
