using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class ProduceBeamsDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsDate")]
        public DateTimeOffset ProduceBeamsDate { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsTime")]
        public TimeSpan ProduceBeamsTime { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsShift")]
        public ShiftId ProduceBeamsShift { get; set; }

        [JsonProperty(PropertyName = "ProduceBeamsOperator")]
        public OperatorId ProduceBeamsOperator { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamLength")]
        public double WarpingBeamLength { get; set; }

        [JsonProperty(PropertyName = "Tention")]
        public int Tention { get; set; }

        [JsonProperty(PropertyName = "MachineSpeed")]
        public int MachineSpeed { get; set; }

        [JsonProperty(PropertyName = "PressRoll")]
        public double PressRoll { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceBeamsDailyOperationWarpingCommandValidator
        : AbstractValidator<ProduceBeamsDailyOperationWarpingCommand>
    {
        public ProduceBeamsDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ProduceBeamsDate).NotEmpty();
            RuleFor(command => command.ProduceBeamsTime).NotEmpty();
            RuleFor(command => command.ProduceBeamsShift).NotEmpty();
            RuleFor(command => command.ProduceBeamsOperator).NotEmpty();
            RuleFor(command => command.WarpingBeamLength).NotEmpty();
            RuleFor(command => command.Tention).NotEmpty();
            RuleFor(command => command.MachineSpeed).NotEmpty();
            RuleFor(command => command.PressRoll).NotEmpty();
        }
    }
}
