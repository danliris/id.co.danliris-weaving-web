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

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "TexSQ")]
        public string TexSQ { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; set; }

        //[JsonProperty(PropertyName = "SizingDetails")]
        //public UpdateDoffFinishDailyOperationSizingDetailCommand Details { get; set; }

        [JsonProperty(PropertyName = "FinishDoffDate")]
        public DateTimeOffset FinishDoffDate { get; set; }

        [JsonProperty(PropertyName = "FinishDoffTime")]
        public TimeSpan FinishDoffTime { get; set; }

        [JsonProperty(PropertyName = "FinishDoffShift")]
        public ShiftId FinishDoffShift { get; set; }

        [JsonProperty(PropertyName = "FinishDoffOperator")]
        public OperatorId FinishDoffOperator { get; set; }

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
            RuleFor(validator => validator.MachineSpeed).NotEmpty().WithMessage("Kecepatan Mesin Harus Diisi");
            RuleFor(validator => validator.TexSQ).NotEmpty().WithMessage("TexSQ Harus Diisi");
            RuleFor(validator => validator.Visco).NotEmpty().WithMessage("Visco Harus Diisi");
            RuleFor(validator => validator.FinishDoffDate).NotEmpty().WithMessage("Tanggal Selesai Harus Diisi");
            RuleFor(validator => validator.FinishDoffTime).NotEmpty().WithMessage("Waktu Selesai Harus Diisi");
            RuleFor(validator => validator.FinishDoffShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.FinishDoffOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
