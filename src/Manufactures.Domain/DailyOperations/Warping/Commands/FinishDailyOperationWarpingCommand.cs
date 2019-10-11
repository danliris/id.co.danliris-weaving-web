using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class FinishDailyOperationWarpingCommand
         : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "FinishDate")]
        public DateTimeOffset FinishDate { get; set; }

        [JsonProperty(PropertyName = "FinishTime")]
        public TimeSpan FinishTime { get; set; }

        [JsonProperty(PropertyName = "FinishShift")]
        public ShiftId FinishShift { get; set; }

        [JsonProperty(PropertyName = "FinishOperator")]
        public OperatorId FinishOperator { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class FinishDailyOperationWarpingCommandValidator
         : AbstractValidator<FinishDailyOperationWarpingCommand>
    {
        public FinishDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.FinishDate).NotEmpty().WithMessage("Tanggal Selesai Harus Diisi");
            RuleFor(command => command.FinishTime).NotEmpty().WithMessage("Waktu Selesai Harus Diisi");
            RuleFor(command => command.FinishShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.FinishOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
