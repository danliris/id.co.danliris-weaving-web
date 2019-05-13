using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Shifts.Commands
{
    public class AddShiftCommand : ICommand<ShiftDocument>
    {
        [JsonProperty(propertyName: "Name")]
        public string Name { get; private set; }

        [JsonProperty(propertyName: "StartTime")]
        public string StartTime { get; private set; }

        [JsonProperty(propertyName: "EndTime")]
        public string EndTime { get; private set; }
    }

    public class AddShiftCommandValidator : AbstractValidator<AddShiftCommand>
    {
        public AddShiftCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.StartTime).NotEmpty();
            RuleFor(command => command.EndTime).NotEmpty();
        }
    }
}
