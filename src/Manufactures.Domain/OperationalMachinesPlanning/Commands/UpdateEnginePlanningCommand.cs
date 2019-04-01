
using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.OperationalMachinesPlanning.Commands
{
    public class UpdateEnginePlanningCommand : ICommand<MachinesPlanningDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Area")]
        public string Area { get; set; }

        [JsonProperty(PropertyName = "Blok")]
        public string Blok { get; set; }

        [JsonProperty(PropertyName = "BlokKaizen")]
        public string BlokKaizen { get; set; }

        [JsonProperty(PropertyName = "UnitDepartementId")]
        public int UnitDepartementId { get; set; }

        [JsonProperty(PropertyName = "MachineId")]
        public Guid MachineId { get; set; }

        [JsonProperty(PropertyName = "UserMaintenanceId")]
        public string UserMaintenanceId { get; set; }

        [JsonProperty(PropertyName = "UserOperatorId")]
        public string UserOperatorId { get; set; }

        public void SetId(Guid identity)
        {
            Id = identity;
        }
    }

    public class UpdateEnginePlanningCommandValidator : AbstractValidator<UpdateEnginePlanningCommand>
    {
        public UpdateEnginePlanningCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.Area).NotEmpty();
            RuleFor(command => command.Blok).NotEmpty();
            RuleFor(command => command.BlokKaizen).NotEmpty();
            RuleFor(command => command.UnitDepartementId).NotEmpty();
            RuleFor(command => command.MachineId).NotEmpty();
            RuleFor(command => command.UserMaintenanceId).NotEmpty();
            RuleFor(command => command.UserOperatorId).NotEmpty();
        }
    }
}
