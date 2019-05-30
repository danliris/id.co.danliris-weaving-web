using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class AddNewDailyOperationLoomCommand 
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "MachineId")]
        public MachineId MachineId { get; set; }

        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; private set; }

        [JsonProperty(PropertyName = "OrderId")]
        public OrderId OrderId { get; private set; }

        [JsonProperty(PropertyName = "PreparationDate")]
        public DateTimeOffset PreparationDate { get; private set; }

        [JsonProperty(PropertyName = "PreparationTime")]
        public TimeSpan PreparationTime { get; private set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; private set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationMonitoringId")]
        public DailyOperationMonitoringId DailyOperationMonitoringId { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; private set; }
    }

    public class AddNewDailyOperationalMachineCommandValidator 
        : AbstractValidator<AddNewDailyOperationLoomCommand>
    {
        public AddNewDailyOperationalMachineCommandValidator()
        {
            RuleFor(command => command.UnitId).NotEmpty();
            RuleFor(command => command.MachineId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.OrderId.Value).NotEmpty();
            RuleFor(command => command.PreparationDate).NotEmpty();
            RuleFor(command => command.PreparationTime).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.WarpOrigin).NotEmpty();
            RuleFor(command => command.WeftOrigin).NotEmpty();
        }
    }
}
