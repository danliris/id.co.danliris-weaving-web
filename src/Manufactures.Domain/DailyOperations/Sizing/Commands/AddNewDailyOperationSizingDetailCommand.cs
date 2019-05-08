using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class AddNewDailyOperationSizingDetailCommand
    {

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public string ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ProductionTime")]
        public DailyOperationSizingProductionTimeCommand ProductionTime { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public DailyOperationSizingCounterCommand Counter { get; set; }

        [JsonProperty(PropertyName = "Weight")]
        public DailyOperationSizingWeightCommand Weight { get; set; }

        [JsonProperty(PropertyName = "WarpingBeamCollectionDocumentId")]
        public List<BeamId> WarpingBeamCollectionDocumentId { get; set; }
    }

    public class AddNewDailyOperationSizingDetailCommandValidator : AbstractValidator<AddNewDailyOperationSizingDetailCommand>
    {
        public AddNewDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(command => command.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(command => command.ShiftId).NotEmpty();
            RuleFor(command => command.OperatorDocumentId.Value).NotEmpty();
            RuleFor(command => command.ProductionTime.Start).NotEmpty();
            RuleFor(command => command.Counter.Start).NotEmpty();
            RuleFor(command => command.Weight.Netto).NotEmpty();
            RuleFor(command => command.WarpingBeamCollectionDocumentId.Count).Equal(0);
        }
    }
}
