using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Machines.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.Machines.Commands
{
    public class AddNewMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }
        public string Location { get; set; }
        public MachineTypeValueObject MachineType { get; set; }
    }

    public class AddNewMachineCommandValidator : AbstractValidator<AddNewMachineCommand>
    {
        public AddNewMachineCommandValidator()
        {
            RuleFor(r => r.MachineNumber).NotNull();
            RuleFor(r => r.Location).NotNull();
            RuleFor(r => r.MachineType.Id).NotNull();
            RuleFor(r => r.MachineType.Name).NotNull();
        }
    }
}
