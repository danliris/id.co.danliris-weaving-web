using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdatePauseDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "PauseDate")]
        public DateTimeOffset PauseDate { get; set; }

        [JsonProperty(PropertyName = "PauseTime")]
        public TimeSpan PauseTime { get; set; }

        [JsonProperty(PropertyName = "PauseShift")]
        public ShiftId PauseShift { get; set; }

        [JsonProperty(PropertyName = "PauseOperator")]
        public OperatorId PauseOperator { get; set; }

        [JsonProperty(PropertyName = "BrokenBeam")]
        public int BrokenBeam { get; set; }

        [JsonProperty(PropertyName = "MachineTroubled")]
        public int MachineTroubled { get; set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdatePauseDailyOperationSizingCommandValidator : AbstractValidator<UpdatePauseDailyOperationSizingCommand>
    {
        public UpdatePauseDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.PauseDate).NotEmpty().WithMessage("Tanggal Berhenti Harus Diisi");
            RuleFor(validator => validator.PauseTime).NotEmpty().WithMessage("Waktu Berhenti Harus Diisi");
            RuleFor(validator => validator.PauseShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.PauseOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
