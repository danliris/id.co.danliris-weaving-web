using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.BeamStockMonitoring.Commands
{
    public class BeamStockMonitoringCommand : ICommand<BeamStockMonitoringDocument>
    {
        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        [JsonProperty(PropertyName = "EntryDate")]
        public DateTimeOffset EntryDate { get; set; }

        [JsonProperty(PropertyName = "EntryTime")]
        public TimeSpan EntryTime { get; set; }

        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "LengthStock")]
        public double LengthStock { get; set; }

        //[JsonProperty(PropertyName = "LengthUOMId")]
        //public int LengthUOMId { get; set; }
    }
    public class SizingBeamStockMonitoringCommandValidator : AbstractValidator<BeamStockMonitoringCommand>
    {
        public SizingBeamStockMonitoringCommandValidator()
        {
            RuleFor(command => command.BeamDocumentId).NotEmpty();
            RuleFor(command => command.EntryDate).NotEmpty();
            RuleFor(command => command.EntryTime).NotEmpty();
            RuleFor(command => command.OrderDocumentId).NotEmpty();
            RuleFor(command => command.LengthStock).NotEmpty();
            //RuleFor(command => command.LengthUOMId).NotEmpty();
        }
    }
}
