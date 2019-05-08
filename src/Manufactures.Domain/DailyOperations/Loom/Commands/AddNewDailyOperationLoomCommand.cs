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
        [JsonProperty(PropertyName = "DateOperated")]
        public DateTimeOffset DateOperated { get; private set; }

        [JsonProperty(PropertyName = "MachineId")]
        public MachineId MachineId { get; set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "DailyOperationSizingId")]
        public DailyOperationSizingId DailyOperationSizingId { get; set; }

        [JsonProperty(PropertyName = "DailyOperationStatus")]
        public string DailyOperationStatus { get; set; }

        [JsonProperty(PropertyName = "DailyOperationMachineDetails")]
        public DailyOperationLoomDetailCommand Detail { get; set; }
    }

    public class AddNewDailyOperationalMachineCommandValidator 
        : AbstractValidator<AddNewDailyOperationLoomCommand>
    {
        public AddNewDailyOperationalMachineCommandValidator()
        {
            RuleFor(command => command.DateOperated).NotEmpty();
            RuleFor(command => command.MachineId.Value).NotEmpty();
            RuleFor(command => command.UnitId.Value).NotEmpty();
            RuleFor(command => command.DailyOperationSizingId.Value).NotEmpty();
            RuleFor(command => command.DailyOperationStatus).NotEmpty();
            RuleFor(command => command.Detail)
                .SetValidator(new DailyOperationLoomDetailCommandValidator());
        }
    }
}
