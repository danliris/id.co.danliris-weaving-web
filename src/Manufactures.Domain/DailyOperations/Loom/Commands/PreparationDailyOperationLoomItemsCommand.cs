using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class PreparationDailyOperationLoomItemsCommand
    {
        [JsonProperty(PropertyName = "BeamOrigin")]
        public string BeamOrigin { get; set; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public Guid BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(PropertyName = "TyingMachineId")]
        public Guid TyingMachineId { get; set; }

        [JsonProperty(PropertyName = "TyingOperatorId")]
        public Guid TyingOperatorId { get; set; }

        [JsonProperty(PropertyName = "LoomMachineId")]
        public Guid LoomMachineId { get; set; }

        [JsonProperty(PropertyName = "LoomOperatorId")]
        public Guid LoomOperatorId { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "PreparationShift")]
        public Guid PreparationShift { get; set; }
    }

    public class PreparationDailyOperationLoomItemsCommandValidator : AbstractValidator<PreparationDailyOperationLoomItemsCommand>
    {
        public PreparationDailyOperationLoomItemsCommandValidator()
        {
            RuleFor(validator => validator.BeamOrigin).NotEmpty().WithMessage("Asal Beam Harus Diisi");
            RuleFor(validator => validator.BeamDocumentId).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.LoomMachineId).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.LoomOperatorId).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationDate).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationTime).NotEmpty().WithMessage("Waktu Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationShift).NotEmpty().WithMessage("Shift Harus Diisi");
        }
    }
}
