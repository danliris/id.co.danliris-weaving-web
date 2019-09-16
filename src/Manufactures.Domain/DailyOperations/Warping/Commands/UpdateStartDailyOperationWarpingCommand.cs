using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class UpdateStartDailyOperationWarpingCommand
        : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "WarpingStartDate")]
        public DateTimeOffset WarpingStartDate { get; set; }

        [JsonProperty(PropertyName = "WarpingStartTime")]
        public TimeSpan WarpingStartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamId")]
        public BeamId WarpingBeamId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateStartDailyOperationWarpingCommandValidator
        : AbstractValidator<UpdateStartDailyOperationWarpingCommand>
    {
        public UpdateStartDailyOperationWarpingCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.WarpingBeamId.Value).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.WarpingStartDate).NotEmpty();
            RuleFor(command => command.WarpingStartTime).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
        }
    }
}
