using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdateResumeDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ResumeBeamProductBeamId")]
        public Guid ResumeBeamProductBeamId { get; set; }

        [JsonProperty(PropertyName = "ResumeBeamNumber")]
        public string ResumeBeamNumber { get; set; }

        [JsonProperty(PropertyName = "ResumeMachineNumber")]
        public string ResumeMachineNumber { get; set; }

        [JsonProperty(PropertyName = "ResumeDateMachine")]
        public DateTimeOffset ResumeDateMachine { get; set; }

        [JsonProperty(PropertyName = "ResumeTimeMachine")]
        public TimeSpan ResumeTimeMachine { get; set; }

        [JsonProperty(PropertyName = "ResumeShiftDocumentId")]
        public ShiftId ResumeShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "ResumeOperatorDocumentId")]
        public OperatorId ResumeOperatorDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateResumeDailyOperationLoomCommandValidator : AbstractValidator<UpdateResumeDailyOperationLoomCommand>
    {
        public UpdateResumeDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ResumeBeamProductBeamId).NotEmpty();
            RuleFor(validator => validator.ResumeBeamNumber).NotEmpty();
            RuleFor(validator => validator.ResumeMachineNumber).NotEmpty();
            RuleFor(validator => validator.ResumeDateMachine).NotEmpty().WithMessage("Tanggal Lanjutkan Harus Diisi");
            RuleFor(validator => validator.ResumeTimeMachine).NotEmpty().WithMessage("Waktu Lanjutkan Harus Diisi");
            RuleFor(validator => validator.ResumeShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
            RuleFor(validator => validator.ResumeOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
