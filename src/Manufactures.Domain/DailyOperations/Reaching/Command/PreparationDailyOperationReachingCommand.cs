using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class PreparationDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }
    }

    public class PreparationDailyOperationReachingCommandValidator : AbstractValidator<PreparationDailyOperationReachingCommand>
    {
        public PreparationDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.MachineDocumentId).NotEmpty().WithMessage("No. Mesin Harus Diisi");
            RuleFor(validator => validator.OrderDocumentId).NotEmpty().WithMessage("No. Order Produksi Harus Diisi");
            RuleFor(validator => validator.SizingBeamId).NotEmpty().WithMessage("No. Beam Sizing Harus Diisi");
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.PreparationDate).NotEmpty().WithMessage("Tanggal Pasang Harus Diisi");
            RuleFor(validator => validator.PreparationTime).NotEmpty().WithMessage("Waktu Pasang Harus Diisi");
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
