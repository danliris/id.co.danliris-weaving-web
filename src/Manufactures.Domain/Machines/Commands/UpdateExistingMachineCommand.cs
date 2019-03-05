using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Machines.Commands
{
    public class UpdateExistingMachineCommand : ICommand<MachineDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "Location")]
        public string Location { get; set; }

        [JsonProperty(PropertyName = "MachineTypeId")]
        public string MachineTypeId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public string WeavingUnitId { get; set; }
    }

    public class UpdateExistingMachineCommandValidator : AbstractValidator<UpdateExistingMachineCommand>
    {
        public UpdateExistingMachineCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.MachineNumber).NotEmpty();
            RuleFor(r => r.Location).NotEmpty();
            RuleFor(r => r.MachineTypeId).NotEmpty();
            RuleFor(r => r.WeavingUnitId).NotEmpty();
        }
    }
}
