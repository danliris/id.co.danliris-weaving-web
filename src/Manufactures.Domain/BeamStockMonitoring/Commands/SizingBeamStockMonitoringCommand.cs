using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.BeamStockMonitoring.Commands
{
    public class SizingBeamStockMonitoringCommand : ICommand<BeamStockMonitoringDocument>
    {
        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "SizingEntryDate")]
        public DateTimeOffset SizingEntryDate { get; set; }

        [JsonProperty(PropertyName = "SizingEntryTime")]
        public TimeSpan SizingEntryTime { get; set; }

        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "SizingLengthStock")]
        public double SizingLengthStock { get; set; }

        [JsonProperty(PropertyName = "LengthUOMId")]
        public int LengthUOMId { get; set; }
    }
    public class SizingBeamStockMonitoringCommandValidator : AbstractValidator<SizingBeamStockMonitoringCommand>
    {
        public SizingBeamStockMonitoringCommandValidator()
        {
            RuleFor(command => command.BeamDocumentId).NotEmpty();
            RuleFor(command => command.SizingEntryDate).NotEmpty();
            RuleFor(command => command.SizingEntryTime).NotEmpty();
            RuleFor(command => command.OrderDocumentId).NotEmpty();
            RuleFor(command => command.SizingLengthStock).NotEmpty();
            RuleFor(command => command.LengthUOMId).NotEmpty();
        }
    }
}
