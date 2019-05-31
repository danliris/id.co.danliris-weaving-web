using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class StartDailyOperationLoomCommand
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; set; }

        [JsonProperty(PropertyName = "StartDate")]
        public DateTimeOffset StartDate { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public TimeSpan StartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class StartDailyOperationLoomCommandValidator 
        : AbstractValidator<StartDailyOperationLoomCommand>
    {
        public StartDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.BeamId).NotEmpty();
            RuleFor(command => command.StartDate).NotEmpty();
            RuleFor(command => command.StartTime).NotEmpty();
            RuleFor(command => command.ShiftId).NotEmpty();
            RuleFor(command => command.OperatorId).NotEmpty();
        }
    }
}
