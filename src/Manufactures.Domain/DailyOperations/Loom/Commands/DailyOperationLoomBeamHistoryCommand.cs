using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomBeamHistoryCommand
    {
        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "DateMachine")]
        public DateTimeOffset DateMachine { get; set; }

        [JsonProperty(PropertyName = "TimeMachine")]
        public TimeSpan TimeMachine { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "Process")]
        public string Process { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }
    }

    public class DailyOperationLoomBeamHistoryCommandValidator : AbstractValidator<DailyOperationLoomBeamHistoryCommand>
    {
        public DailyOperationLoomBeamHistoryCommandValidator()
        {
            RuleFor(validator => validator.BeamDocumentId).NotEmpty().WithMessage("No. Beam Sizing Harus Diisi");
            RuleFor(validator => validator.MachineDocumentId).NotEmpty().WithMessage("No. Mesin Harus Diisi");
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.DateMachine).NotEmpty().WithMessage("Tanggal Harus Diisi");
            RuleFor(validator => validator.TimeMachine).NotEmpty().WithMessage("Jam Harus Diisi");
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
            RuleFor(validator => validator.Process).NotEmpty().WithMessage("Proses Harus Diisi");
        }
    }
}
