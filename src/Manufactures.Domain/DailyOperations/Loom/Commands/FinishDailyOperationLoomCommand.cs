using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class FinishDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "FinishBeamNumber")]
        public string FinishBeamNumber { get; set; }

        [JsonProperty(PropertyName = "FinishMachineNumber")]
        public string FinishMachineNumber { get; set; }

        [JsonProperty(PropertyName = "FinishDateMachine")]
        public DateTimeOffset FinishDateMachine { get; set; }

        [JsonProperty(PropertyName = "FinishTimeMachine")]
        public TimeSpan FinishTimeMachine { get; set; }

        [JsonProperty(PropertyName = "FinishShiftDocumentId")]
        public ShiftId FinishShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "FinishOperatorDocumentId")]
        public OperatorId FinishOperatorDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class FinishDailyOperationLoomCommandValidator : AbstractValidator<FinishDailyOperationLoomCommand>
    {
        public FinishDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.FinishBeamNumber).NotEmpty();
            RuleFor(validator => validator.FinishMachineNumber).NotEmpty();
            RuleFor(validator => validator.FinishDateMachine).NotEmpty().WithMessage("Tanggal Selesai Harus Diisi");
            RuleFor(validator => validator.FinishTimeMachine).NotEmpty().WithMessage("Waktu Selesai Harus Diisi");
            RuleFor(validator => validator.FinishShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
            RuleFor(validator => validator.FinishOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
