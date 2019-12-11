using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class PreparationDailyOperationLoomBeamProductCommand
    {

        [JsonProperty(PropertyName = "BeamOrigin")]
        public string BeamOrigin { get; set; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "CombNumber")]
        public double CombNumber { get; set; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "DateBeamProduct")]
        public DateTimeOffset DateBeamProduct { get; set; }

        [JsonProperty(PropertyName = "TimeBeamProduct")]
        public TimeSpan TimeBeamProduct { get; set; }

        [JsonProperty(PropertyName = "LoomProcess")]
        public string LoomProcess { get; set; }
    }

    public class PreparationDailyOperationLoomBeamProductCommandValidator : AbstractValidator<PreparationDailyOperationLoomBeamProductCommand>
    {
        public PreparationDailyOperationLoomBeamProductCommandValidator()
        {
            RuleFor(validator => validator.BeamOrigin).NotEmpty().WithMessage("Asal Beam Harus Diisi");
            RuleFor(validator => validator.BeamDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.CombNumber).NotEmpty().WithMessage("No. Sisir Harus Diisi");
            RuleFor(validator => validator.MachineDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.DateBeamProduct).NotEmpty().WithMessage("Tanggal Harus Diisi");
            RuleFor(validator => validator.TimeBeamProduct).NotEmpty().WithMessage("Jam Harus Diisi");
            RuleFor(validator => validator.LoomProcess).NotEmpty().WithMessage("Proses Tidak Boleh Kosong");
        }
    }
}
