using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateResumeDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        //[JsonProperty(PropertyName = "SizingDetails")]
        //public UpdateResumeDailyOperationSizingDetailCommand Details { get; set; }

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

    public class UpdateResumeDailyOperationSizingCommandValidator : AbstractValidator<UpdateResumeDailyOperationSizingCommand>
    {
        public UpdateResumeDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            //RuleFor(validator => validator.Details).SetValidator(new UpdateResumeDailyOperationSizingDetailCommandValidator());
            RuleFor(command => command.ResumeDate).NotEmpty().WithMessage("Tanggal Lanjutkan Harus Diisi");
            RuleFor(command => command.ResumeTime).NotEmpty().WithMessage("Waktu Lanjutkan Harus Diisi");
            RuleFor(command => command.ResumeShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.ResumeOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
