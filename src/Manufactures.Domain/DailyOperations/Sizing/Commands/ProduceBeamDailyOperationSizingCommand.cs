using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ProduceBeamDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
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

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDailyOperationSizingCommand>
    {
        public ProduceBeamDailyOperationSizingCommandValidator()
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
        }
    }
}
