using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "CounterStart")]
        public double CounterStart { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "StartShift")]
        public ShiftId StartShift { get; set; }

        [JsonProperty(PropertyName = "StartOperator")]
        public OperatorId StartOperator { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class AddNewDailyOperationSizingCommandValidator : AbstractValidator<UpdateStartDailyOperationSizingCommand>
    {
        public AddNewDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.SizingBeamId).NotEmpty().WithMessage("No. Beam Sizing Harus Diisi");
            RuleFor(validator => validator.CounterStart).GreaterThanOrEqualTo(0).WithMessage("Counter Awal Harus Lebih Besar atau Sama Dengan 0");
            RuleFor(validator => validator.StartDate).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(validator => validator.StartTime).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(validator => validator.StartShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(validator => validator.StartOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
