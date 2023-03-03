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

        [JsonProperty(PropertyName = "ReprocessBeamUsedId")]
        public Guid ReprocessBeamUsedId { get; set; }

        [JsonProperty(PropertyName = "ReprocessBeamUsedNumber")]
        public string ReprocessBeamUsedNumber { get; set; }

        [JsonProperty(PropertyName = "ReprocessTyingOperatorDocumentId")]
        public Guid ReprocessTyingOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ReprocessTyingOperatorName")]
        public string ReprocessTyingOperatorName { get; set; }

        [JsonProperty(PropertyName = "ReprocessLoomOperatorDocumentId")]
        public Guid ReprocessLoomOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ReprocessDate")]
        public DateTimeOffset ReprocessDate { get; set; }

        [JsonProperty(PropertyName = "ReprocessTime")]
        public TimeSpan ReprocessTime { get; set; }

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
            RuleFor(validator => validator.ReprocessBeamUsedId).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.ReprocessBeamUsedNumber).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.ReprocessLoomOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.ReprocessDate).NotEmpty().WithMessage("Tanggal Reproses Harus Diisi");
            RuleFor(validator => validator.ReprocessTime).NotEmpty().WithMessage("Waktu Reproses Harus Diisi");
            RuleFor(validator => validator.ReprocessShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
