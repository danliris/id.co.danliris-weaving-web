using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class NewEntryDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public UnitId WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "PISPieces")]
        public double PISPieces { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }
    }

    public class NewEntryDailyOperationReachingCommandValidator : AbstractValidator<NewEntryDailyOperationReachingCommand>
    {
        public NewEntryDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.MachineDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.WeavingUnitId.Value).NotEmpty();
            RuleFor(validator => validator.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.SizingBeamId.Value).NotEmpty();
            RuleFor(validator => validator.PISPieces).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.PreparationDate).NotEmpty();
            RuleFor(validator => validator.PreparationTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
