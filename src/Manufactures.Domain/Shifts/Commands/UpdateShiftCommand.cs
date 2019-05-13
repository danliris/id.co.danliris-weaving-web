using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Shifts.Commands
{
    public class UpdateShiftCommand : ICommand<ShiftDocument>
    {
        [JsonProperty(propertyName: "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(propertyName: "Name")]
        public string Name { get; private set; }

        [JsonProperty(propertyName: "StartTime")]
        public string StartTime { get; private set; }

        [JsonProperty(propertyName: "EndTime")]
        public string EndTime { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateShiftCommandValidator : AbstractValidator<UpdateShiftCommand>
    {
        public UpdateShiftCommandValidator()
        {
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.StartTime).NotEmpty();
            RuleFor(command => command.EndTime).NotEmpty();
        }
    }
}
