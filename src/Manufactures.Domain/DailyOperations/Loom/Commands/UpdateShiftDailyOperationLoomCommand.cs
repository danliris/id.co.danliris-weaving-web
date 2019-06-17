using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdateShiftDailyOperationLoomCommand
        : ICommand<string>
    {
        public string Command { get; set; }
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ChangeShiftDate")]
        public DateTimeOffset ChangeShiftDate { get; set; }

        [JsonProperty(PropertyName = "ChangeShifTime")]
        public TimeSpan ChangeShifTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class UpdateShiftDailyOperationLoomCommandValidator
        : AbstractValidator<UpdateShiftDailyOperationLoomCommand>
    {
        public UpdateShiftDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ChangeShiftDate).NotEmpty();
            RuleFor(command => command.ChangeShifTime).NotEmpty();
            RuleFor(command => command.ShiftId).NotEmpty();
            RuleFor(command => command.OperatorId).NotEmpty();
        }
    }
}