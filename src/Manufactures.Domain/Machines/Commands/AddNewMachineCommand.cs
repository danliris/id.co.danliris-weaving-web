using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Machines.Commands
{
    public class AddNewMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "MachineTypeId")]
        public string MachineTypeId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public string WeavingUnitId { get; set; }
    }

    public class AddNewMachineCommandValidator : AbstractValidator<AddNewMachineCommand>
    {
        public AddNewMachineCommandValidator()
        {
            RuleFor(r => r.MachineNumber).NotEmpty();
            RuleFor(r => r.Location).NotEmpty();
            RuleFor(r => r.MachineTypeId).NotEmpty();
            RuleFor(r => r.WeavingUnitId).NotEmpty();
        }
    }
}
