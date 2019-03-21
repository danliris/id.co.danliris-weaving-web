using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.MachineTypes.Commands
{
    public class AddNewMachineTypeCommand : ICommand<MachineTypeDocument>
    {
        [JsonProperty(PropertyName = "TypeName")]
        public string TypeName { get; set; }

        [JsonProperty(PropertyName = "Speed")]
        public int Speed { get; set; }

        [JsonProperty(PropertyName = "MachineUnit")]
        public string MachineUnit { get; set; }
    }

    public class AddNewMachineTypeCommandValidator : AbstractValidator<AddNewMachineTypeCommand>
    {
        public AddNewMachineTypeCommandValidator()
        {
            RuleFor(r => r.TypeName).NotEmpty();
            RuleFor(r => r.Speed).NotEmpty();
            RuleFor(r => r.MachineUnit).NotEmpty();
        }
    }
}
