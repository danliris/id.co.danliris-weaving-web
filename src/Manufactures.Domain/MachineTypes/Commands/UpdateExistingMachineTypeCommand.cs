using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.MachineTypes.Commands
{
    public class UpdateExistingMachineTypeCommand : ICommand<MachineTypeDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "TypeName")]
        public string TypeName { get; set; }

        [JsonProperty(PropertyName = "Speed")]
        public int Speed { get; set; }

        [JsonProperty(PropertyName = "MachineUnit")]
        public string MachineUnit { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateExistingMachineTypeCommandValidator : AbstractValidator<UpdateExistingMachineTypeCommand>
    {
        public UpdateExistingMachineTypeCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.TypeName).NotEmpty();
            RuleFor(r => r.Speed).NotEmpty();
            RuleFor(r => r.MachineUnit).NotEmpty();
        }
    }
}
