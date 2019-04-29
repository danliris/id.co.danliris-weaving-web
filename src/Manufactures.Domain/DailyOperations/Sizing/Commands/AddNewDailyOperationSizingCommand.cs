using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class AddNewDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "ProductionDate")]
        public DateTimeOffset ProductionDate { get; private set; }

        [JsonProperty(PropertyName = "MachineDocumentId")]
        public MachineId MachineDocumentId { get; set; }

        [JsonProperty(PropertyName = "WeavingUnitId")]
        public UnitId WeavingUnitId { get; set; }

        [JsonProperty(PropertyName = "DailyOperationSizingDetails")]
        public List<DailyOperationSizingDetailCommand> DailyOperationSizingDetails { get; set; }
    }

    public class AddNewDailyOperationalMachineCommandValidator
        : AbstractValidator<AddNewDailyOperationSizingCommand>
    {
        public AddNewDailyOperationalMachineCommandValidator()
        {
            RuleFor(command => command.ProductionDate).NotEmpty();
            RuleFor(command => command.MachineDocumentId.Value).NotEmpty();
            RuleFor(command => command.WeavingUnitId.Value).NotEmpty();
        }
    }
}
