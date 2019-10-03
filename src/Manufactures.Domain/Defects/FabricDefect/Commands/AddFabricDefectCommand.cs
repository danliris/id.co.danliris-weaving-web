using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Defects.FabricDefect.Commands
{
    public class AddFabricDefectCommand : ICommand<FabricDefectDocument>
    {
        [JsonProperty(propertyName: "DefectCode")]
        public string DefectCode { get; set; }

        [JsonProperty(propertyName: "DefectType")]
        public string DefectType { get; set; }

        [JsonProperty(propertyName: "DefectCategory")]
        public string DefectCategory { get; set; }
    }

    public class AddYarnDefectCommandValidator : AbstractValidator<AddFabricDefectCommand>
    {
        public AddYarnDefectCommandValidator()
        {
            RuleFor(command => command.DefectCode).NotEmpty();
            RuleFor(command => command.DefectType).NotEmpty();
            RuleFor(command => command.DefectCategory).NotEmpty();
        }
    }
}
