using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class StopDailyOperationLoomCommand 
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "StopDate")]
        public DateTimeOffset StopDate { get; set; }

        [JsonProperty(PropertyName = "StopTime")]
        public TimeSpan StopTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class StopDailyOperationLoomCommandValidator 
        : AbstractValidator<StopDailyOperationLoomCommand>
    {
        public StopDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.StopDate).NotEmpty();
            RuleFor(command => command.StopTime).NotEmpty();
            RuleFor(command => command.ShiftId).NotEmpty();
            RuleFor(command => command.OperatorId).NotEmpty();
        }
    }
}
