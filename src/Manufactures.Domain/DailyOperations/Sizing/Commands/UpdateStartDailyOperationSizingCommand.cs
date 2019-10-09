using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        //[JsonProperty(PropertyName = "SizingBeamDocuments")]
        //public UpdateStartDailyOperationSizingBeamDocumentCommand SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingBeamId")]
        public BeamId SizingBeamId { get; set; }

        [JsonProperty(PropertyName = "Start")]
        public double Start { get; set; }

        //[JsonProperty(PropertyName = "Finish")]
        //public double Finish { get; set; }

        //[JsonProperty(PropertyName = "SizingDetails")]
        //public UpdateStartDailyOperationSizingDetailCommand SizingDetails { get; set; }

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
            //RuleFor(validator => validator.SizingBeamDocuments).SetValidator(new UpdateStartDailyOperationSizingBeamDocumentCommandValidator());
            RuleFor(validator => validator.SizingBeamId).NotEmpty();
            RuleFor(validator => validator.Start >= 0);
            //RuleFor(validator => validator.Finish).NotEmpty().Unless(validator => validator.Start >= 0);
            //RuleFor(validator => validator.SizingDetails).SetValidator(new UpdateStartDailyOperationSizingDetailCommandValidator());
            RuleFor(command => command.StartDate).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(command => command.StartTime).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(command => command.StartShift).NotEmpty().WithMessage("Shift Harus Diisi");
            RuleFor(command => command.StartOperator).NotEmpty().WithMessage("Operator Harus Diisi");
        }
    }
}
