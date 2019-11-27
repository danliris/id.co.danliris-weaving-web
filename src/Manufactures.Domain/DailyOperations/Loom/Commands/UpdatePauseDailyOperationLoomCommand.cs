using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdatePauseDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamProductId")]
        public Guid BeamProductId { get; set; }

        [JsonProperty(PropertyName = "PauseBeamNumber")]
        public string PauseBeamNumber { get; set; }

        [JsonProperty(PropertyName = "PauseMachineNumber")]
        public string PauseMachineNumber { get; set; }

        [JsonProperty(PropertyName = "WarpBrokenThreads")]
        public int? WarpBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "WeftBrokenThreads")]
        public int? WeftBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "LenoBrokenThreads")]
        public int? LenoBrokenThreads { get; set; }

        [JsonProperty(PropertyName = "ReprocessTo")]
        public string ReprocessTo { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }

        [JsonProperty(PropertyName = "PauseDateMachine")]
        public DateTimeOffset PauseDateMachine { get; set; }

        [JsonProperty(PropertyName = "PauseTimeMachine")]
        public TimeSpan PauseTimeMachine { get; set; }

        [JsonProperty(PropertyName = "PauseShiftDocumentId")]
        public ShiftId PauseShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "PauseOperatorDocumentId")]
        public OperatorId PauseOperatorDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdatePauseDailyOperationLoomCommandValidator : AbstractValidator<UpdatePauseDailyOperationLoomCommand>
    {
        public UpdatePauseDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.PauseBeamNumber).NotEmpty();
            RuleFor(validator => validator.PauseMachineNumber).NotEmpty();
            RuleFor(validator => validator.PauseDateMachine).NotEmpty().WithMessage("Tanggal Jeda Harus Diisi");
            RuleFor(validator => validator.PauseTimeMachine).NotEmpty().WithMessage("Waktu Jeda Harus Diisi");
            RuleFor(validator => validator.PauseShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
            RuleFor(validator => validator.PauseOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
