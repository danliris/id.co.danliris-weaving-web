using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdateStartDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "StartBeamNumber")]
        public string StartBeamNumber { get; set; }

        [JsonProperty(PropertyName = "StartMachineNumber")]
        public string StartMachineNumber { get; set; }

        [JsonProperty(PropertyName = "StartDateMachine")]
        public DateTimeOffset StartDateMachine { get; set; }

        [JsonProperty(PropertyName = "StartTimeMachine")]
        public TimeSpan StartTimeMachine { get; set; }

        [JsonProperty(PropertyName = "StartShiftDocumentId")]
        public ShiftId StartShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "StartOperatorDocumentId")]
        public OperatorId StartOperatorDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateStartDailyOperationLoomCommandValidator : AbstractValidator<UpdateStartDailyOperationLoomCommand>
    {
        public UpdateStartDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.StartBeamNumber).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.StartMachineNumber).NotEmpty().WithMessage("No. Machine Tidak Boleh Kosong");
            RuleFor(validator => validator.StartDateMachine).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(validator => validator.StartTimeMachine).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(validator => validator.StartShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
            RuleFor(validator => validator.StartOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
