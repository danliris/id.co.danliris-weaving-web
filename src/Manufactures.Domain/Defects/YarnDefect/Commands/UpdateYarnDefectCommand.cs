using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Defects.YarnDefect.Commands
{
    public class UpdateYarnDefectCommand : ICommand<YarnDefectDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(propertyName: "DefectCode")]
        public string DefectCode { get; set; }

        [JsonProperty(propertyName: "DefectType")]
        public string DefectType { get; set; }

        [JsonProperty(propertyName: "DefectCategory")]
        public string DefectCategory { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateYarnDefectCommandValidator : AbstractValidator<UpdateYarnDefectCommand>
    {
        public UpdateYarnDefectCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.DefectCode).NotEmpty();
            RuleFor(command => command.DefectType).NotEmpty();
            RuleFor(command => command.DefectCategory).NotEmpty();
        }
    }
}
