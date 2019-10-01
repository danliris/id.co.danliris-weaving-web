using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Defects.YarnDefect.Commands
{
    public class AddYarnDefectCommand : ICommand<YarnDefectDocument>
    {
        [JsonProperty(propertyName: "DefectCode")]
        public string DefectCode { get; set; }

        [JsonProperty(propertyName: "DefectType")]
        public string DefectType { get; set; }

        [JsonProperty(propertyName: "DefectCategory")]
        public string DefectCategory { get; set; }
    }

    public class AddYarnDefectCommandValidator : AbstractValidator<AddYarnDefectCommand>
    {
        public AddYarnDefectCommandValidator()
        {
            RuleFor(command => command.DefectCode).NotEmpty();
            RuleFor(command => command.DefectType).NotEmpty();
            RuleFor(command => command.DefectCategory).NotEmpty();
        }
    }
}
