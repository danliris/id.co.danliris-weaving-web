using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdatePauseDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        //[JsonProperty(PropertyName = "SizingDetails")]
        //public UpdatePauseDailyOperationSizingDetailCommand Details { get; set; }

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
            //RuleFor(validator => validator.Details).SetValidator(new UpdatePauseDailyOperationSizingDetailCommandValidator());
            RuleFor(command => command.PauseDate).NotEmpty().WithMessage("Tanggal Berhenti Harus Diisi");
            RuleFor(command => command.PauseTime).NotEmpty().WithMessage("Waktu Berhenti Harus Diisi");
            RuleFor(command => command.PauseShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.PauseOperator).NotEmpty().WithMessage("Operator Harus Diisi");
            //RuleFor(command => command.BrokenBeam).NotEmpty().Unless(command => string.IsNullOrEmpty(command.MachineTroubled)).WithMessage("Penyebab Berhenti Harus Diisi");
            RuleFor(command => command.BrokenBeam).NotEmpty().WithMessage("Penyebab Berhenti Harus Diisi");
            //RuleFor(command => command.MachineTroubled).NotEmpty().Unless(command => string.IsNullOrEmpty(command.BrokenBeam)).WithMessage("Penyebab Berhenti Harus Diisi");
            RuleFor(command => command.MachineTroubled).NotEmpty().WithMessage("Penyebab Berhenti Harus Diisi");
        }
    }
}
