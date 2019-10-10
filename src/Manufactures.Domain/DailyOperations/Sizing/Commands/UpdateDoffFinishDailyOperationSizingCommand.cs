using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
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

        [JsonProperty(PropertyName = "FinishDate")]
        public DateTimeOffset FinishDate { get; set; }

        [JsonProperty(PropertyName = "FinishTime")]
        public TimeSpan FinishTime { get; set; }

        [JsonProperty(PropertyName = "FinishShift")]
        public ShiftId FinishShift { get; set; }

        [JsonProperty(PropertyName = "FinishOperator")]
        public OperatorId FinishOperator { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
    public class UpdateDoffFinishDailyOperationSizingCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingCommand>
    {
        public UpdateDoffFinishDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.MachineSpeed).NotEmpty();
            RuleFor(validator => validator.TexSQ).NotEmpty();
            RuleFor(validator => validator.Visco).NotEmpty();
            RuleFor(validator => validator.FinishDate).NotEmpty().WithMessage("Tanggal Selesai Harus Diisi");
            RuleFor(validator => validator.FinishTime).NotEmpty().WithMessage("Waktu Selesai Harus Diisi");
            RuleFor(validator => validator.FinishShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.FinishOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
