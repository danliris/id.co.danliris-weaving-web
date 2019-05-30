using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class FinishDailyOperationLoomCommand 
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "FinishDate")]
        public DateTimeOffset FinishDate { get; set; }

        [JsonProperty(PropertyName = "FinishTime")]
        public TimeSpan FinishTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class FinishDailyOperationLoomCommandValidator 
        : AbstractValidator<FinishDailyOperationLoomCommand>
    {
        public FinishDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.FinishDate).NotEmpty();
            RuleFor(command => command.FinishTime).NotEmpty();
            RuleFor(command => command.ShiftId).NotEmpty();
            RuleFor(command => command.OperatorId).NotEmpty();
        }
    }
}
