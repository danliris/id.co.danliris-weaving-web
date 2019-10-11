using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class UpdateResumeDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ResumeDate")]
        public DateTimeOffset ResumeDate { get; set; }

        [JsonProperty(PropertyName = "ResumeTime")]
        public TimeSpan ResumeTime { get; set; }

        [JsonProperty(PropertyName = "ResumeShift")]
        public ShiftId ResumeShift { get; set; }

        [JsonProperty(PropertyName = "ResumeOperator")]
        public OperatorId ResumeOperator { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateResumeDailyOperationWarpingCommandValidator
        : AbstractValidator<UpdateResumeDailyOperationWarpingCommand>
    {
        public UpdateResumeDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ResumeDate).NotEmpty().WithMessage("Tanggal Lanjutkan Harus Diisi");
            RuleFor(command => command.ResumeTime).NotEmpty().WithMessage("Waktu Lanjutkan Harus Diisi");
            RuleFor(command => command.ResumeShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.ResumeOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
