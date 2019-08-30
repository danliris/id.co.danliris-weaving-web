using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.StockCard.Commands
{
    public class CreateStockAdjustmentCommand : ICommand<StockCardDocument>
    {
        [JsonProperty(propertyName: "BeamId")]
        public BeamId BeamId { get; set; }

        [JsonProperty(propertyName: "BeamNumber")]
        public string BeamNumber { get; set; }

        [JsonProperty(propertyName: "EmptyWeight")]
        public double EmptyWeight { get; set; }

        [JsonProperty(propertyName: "YarnLength")]
        public double YarnLength { get; set; }

        [JsonProperty(propertyName: "YarnStrands")]
        public double YarnStrands { get; set; }

        [JsonProperty(propertyName: "DailyOperationId")]
        public DailyOperationId DailyOperationId { get; set; }

        [JsonProperty(propertyName: "DateTimeOperation")]
        public DateTimeOffset DateTimeOperation { get; set; }
    }

    public class CreateStockAdjustmentCommandValidator : AbstractValidator<CreateStockAdjustmentCommand>
    {
        public CreateStockAdjustmentCommandValidator()
        {
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.BeamNumber).NotEmpty();
            RuleFor(command => command.EmptyWeight).NotEmpty();
            RuleFor(command => command.DailyOperationId.Value).NotEmpty();
            RuleFor(command => command.DateTimeOperation).NotNull();
        }
    }
}
