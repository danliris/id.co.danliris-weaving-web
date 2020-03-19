using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class ReprocessDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ReprocessBeamDocumentId")]
        public Guid ReprocessBeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "ReprocessBeamNumber")]
        public string ReprocessBeamNumber { get; set; }

        [JsonProperty(PropertyName = "ReprocessTyingOperatorDocumentId")]
        public Guid ReprocessTyingOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ReprocessLoomOperatorDocumentId")]
        public Guid ReprocessLoomOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ReprocessMachineStatus")]
        public string ReprocessMachineStatus { get; set; }

        [JsonProperty(PropertyName = "ReprocessDateMachine")]
        public DateTimeOffset ReprocessDateMachine { get; set; }

        [JsonProperty(PropertyName = "ReprocessTimeMachine")]
        public TimeSpan ReprocessTimeMachine { get; set; }

        [JsonProperty(PropertyName = "ReprocessShiftDocumentId")]
        public Guid ReprocessShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ReprocessDailyOperationLoomCommandValidator : AbstractValidator<ReprocessDailyOperationLoomCommand>
    {
        public ReprocessDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ReprocessLoomOperatorDocumentId).NotEmpty().WithMessage("Operator Loom Harus Diisi");
            RuleFor(validator => validator.ReprocessDateMachine).NotEmpty().WithMessage("Tanggal Reproses Harus Diisi");
            RuleFor(validator => validator.ReprocessTimeMachine).NotEmpty().WithMessage("Waktu Reproses Harus Diisi");
            RuleFor(validator => validator.ReprocessShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
