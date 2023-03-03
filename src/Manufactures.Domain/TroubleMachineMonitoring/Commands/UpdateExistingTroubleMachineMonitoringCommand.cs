using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.TroubleMachineMonitoring.Commands
{
    public class UpdateExistingTroubleMachineMonitoringCommand : ICommand<TroubleMachineMonitoringDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "StopDate")]
        public DateTimeOffset StopDate { get; set; }

        [JsonProperty(PropertyName = "ContinueDate")]
        public DateTimeOffset ContinueDate { get; set; }

        [JsonProperty(PropertyName = "OrderDocument")]
        public string OrderDocument { get; set; }

        [JsonProperty(PropertyName = "MachineNumber")]
        public string MachineNumber { get; set; }

        [JsonProperty(PropertyName = "OperatorDocument")]
        public string OperatorDocument { get; set; }

        [JsonProperty(PropertyName = "Process")]
        public string Process { get; set; }

        [JsonProperty(PropertyName = "Trouble")]
        public string Trouble { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public string WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateExistingTroubleMachineMonitoringCommandValidator : AbstractValidator<UpdateExistingTroubleMachineMonitoringCommand>
    {
        public UpdateExistingTroubleMachineMonitoringCommandValidator()
        {
            RuleFor(r => r.Id).NotEmpty();
            RuleFor(r => r.MachineNumber).NotEmpty();
            RuleFor(r => r.ContinueDate).NotEmpty();
            RuleFor(r => r.StopDate).NotEmpty();
            RuleFor(r => r.Process).NotEmpty();
            RuleFor(r => r.OperatorDocument).NotEmpty();
            RuleFor(r => r.OrderDocument).NotEmpty();
            RuleFor(r => r.Trouble).NotEmpty();
        }
    }
}
