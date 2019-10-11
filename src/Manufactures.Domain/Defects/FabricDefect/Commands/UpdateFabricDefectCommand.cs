using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Defects.FabricDefect.Commands
{
    public class UpdateFabricDefectCommand : ICommand<FabricDefectDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

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

    public class UpdateYarnDefectCommandValidator : AbstractValidator<UpdateFabricDefectCommand>
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
