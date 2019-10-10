using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class ProduceBeamDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        //[JsonProperty(PropertyName = "SizingBeamDocuments")]
        //public ProduceBeamBeamDocumentDailyOperationSizingCommand SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "CounterFinish")]
        public double CounterFinish { get; set; }

        //[JsonProperty(PropertyName = "Weight")]
        //public DailyOperationSizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "WeightNetto")]
        public double WeightNetto { get; set; }

        [JsonProperty(PropertyName = "WeightBruto")]
        public double WeightBruto { get; set; }

        [JsonProperty(PropertyName = "WeightTheoritical")]
        public double WeightTheoritical { get; set; }

        [JsonProperty(PropertyName = "PISMeter")]
        public double PISMeter { get; set; }

        [JsonProperty(PropertyName = "SPU")]
        public double SPU { get; set; }

        //[JsonProperty(PropertyName = "SizingDetails")]
        //public ProduceBeamDetailDailyOperationSizingCommand SizingDetails { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamOperator")]
        public OperatorId ProduceBeamOperator { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamShift")]
        public ShiftId ProduceBeamShift { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamDate")]
        public DateTimeOffset ProduceBeamDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamTime")]
        public TimeSpan ProduceBeamTime { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamDailyOperationSizingCommandValidator : AbstractValidator<ProduceBeamDailyOperationSizingCommand>
    {
        public ProduceBeamDailyOperationSizingCommandValidator()
        {
            //RuleFor(validator => validator.SizingBeamId.Value).NotEmpty();
            //RuleFor(validator => validator.CounterFinish).NotEmpty();
            //RuleFor(validator => validator.WeightNetto).NotEmpty();
            //RuleFor(validator => validator.WeightBruto).NotEmpty();
            //RuleFor(validator => validator.WeightTheoretical).NotEmpty();
            //RuleFor(validator => validator.SPU).NotEmpty();
            RuleFor(validator => validator.Id).NotEmpty();
            //RuleFor(validator => validator.SizingBeamDocuments).SetValidator(new ProduceBeamBeamDocumentDailyOperationSizingCommandValidator());
            RuleFor(validator => validator.CounterFinish).NotEmpty();
            //RuleFor(validator => validator.Weight).SetValidator(new DailyOperationSizingWeightCommandValidator());
            RuleFor(validator => validator.WeightNetto).NotEmpty();
            RuleFor(validator => validator.WeightBruto).NotEmpty();
            RuleFor(validator => validator.WeightTheoritical).NotEmpty();
            RuleFor(validator => validator.PISMeter).NotEmpty();
            RuleFor(validator => validator.SPU).NotEmpty();
            //RuleFor(validator => validator.SizingDetails).SetValidator(new ProduceBeamDetailDailyOperationSizingCommandValidator());
            RuleFor(validator => validator.ProduceBeamOperator.Value).NotEmpty();
            RuleFor(validator => validator.ProduceBeamShift.Value).NotEmpty();
            RuleFor(validator => validator.ProduceBeamDate).NotEmpty();
            RuleFor(validator => validator.ProduceBeamTime).NotEmpty();
        }
    }
}
