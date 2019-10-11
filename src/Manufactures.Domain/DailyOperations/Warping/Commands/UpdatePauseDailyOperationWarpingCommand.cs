using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class UpdatePauseDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
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

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; set; }

        [JsonProperty(PropertyName = "BrokenThreadsCause")]
        public int BrokenThreadsCause { get; set; }

        [JsonProperty(PropertyName = "ConeDeficient")]
        public int ConeDeficient { get; set; }

        [JsonProperty(PropertyName = "LooseThreadsAmount")]
        public int LooseThreadsAmount { get; set; }

        [JsonProperty(PropertyName = "RightLooseCreel")]
        public int RightLooseCreel { get; set; }

        [JsonProperty(PropertyName = "LeftLooseCreel")]
        public int LeftLooseCreel { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdatePauseDailyOperationWarpingCommandValidator
        : AbstractValidator<UpdatePauseDailyOperationWarpingCommand>
    {
        public UpdatePauseDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.PauseDate).NotEmpty().WithMessage("Tanggal Berhenti Harus Diisi");
            RuleFor(command => command.PauseTime).NotEmpty().WithMessage("Waktu Berhenti Harus Diisi");
            RuleFor(command => command.PauseShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.PauseOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
