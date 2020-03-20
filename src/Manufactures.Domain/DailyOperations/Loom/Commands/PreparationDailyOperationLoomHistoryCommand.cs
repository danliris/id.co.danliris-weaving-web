using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class PreparationDailyOperationLoomHistoryCommand
    {
        [JsonProperty(PropertyName = "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "DateMachine")]
        public DateTimeOffset DateMachine { get; set; }

        [JsonProperty(PropertyName = "TimeMachine")]
        public TimeSpan TimeMachine { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }
    }

    public class PreparationDailyOperationLoomBeamHistoryCommandValidator : AbstractValidator<PreparationDailyOperationLoomHistoryCommand>
    {
        public PreparationDailyOperationLoomBeamHistoryCommandValidator()
        {
            RuleFor(validator => validator.BeamNumber).NotEmpty();
            RuleFor(validator => validator.MachineNumber).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.DateMachine).NotEmpty().WithMessage("Tanggal Harus Diisi");
            RuleFor(validator => validator.TimeMachine).NotEmpty().WithMessage("Jam Harus Diisi");
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
