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

        [JsonProperty(PropertyName = "StartBeamUsedId")]
        public Guid StartBeamUsedId { get; set; }

        [JsonProperty(PropertyName = "StartBeamUsedNumber")]
        public string StartBeamUsedNumber { get; set; }

        [JsonProperty(PropertyName = "StartTyingOperatorDocumentId")]
        public Guid StartTyingOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "StartTyingOperatorName")]
        public string StartTyingOperatorName { get; set; }

        [JsonProperty(PropertyName = "StartLoomOperatorDocumentId")]
        public Guid StartLoomOperatorDocumentId { get; set; }

        //[JsonProperty(PropertyName = "StartCounterPerOperator")]
        //public double StartCounterPerOperator { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "StartShiftDocumentId")]
        public Guid StartShiftDocumentId { get; set; }

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
            RuleFor(validator => validator.StartBeamUsedId).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.StartBeamUsedNumber).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.StartLoomOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.StartDate).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(validator => validator.StartTime).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(validator => validator.StartShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
