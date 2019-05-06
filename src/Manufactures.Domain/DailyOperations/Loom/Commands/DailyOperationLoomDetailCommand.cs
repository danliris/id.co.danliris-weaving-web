using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomDetailCommand
    {
        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; private set; }

        [JsonProperty(PropertyName = "BeamOperatorId")]
        public OperatorId BeamOperatorId { get; private set; }
        
        [JsonProperty(PropertyName = "BeamId")]
        public BeamId BeamId { get; set; }
        
        [JsonProperty(PropertyName = "DOMTime")]
        public DailyOperationLoomHistoryCommand 
            DailyOperationLoomHistory { get; private set; }
        
        [JsonProperty(PropertyName = "WarpOrigin")]
        public Origin WarpOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public Origin WeftOrigin { get; set; }
    }

    public class DailyOperationLoomDetailCommandValidator 
        : AbstractValidator<DailyOperationLoomDetailCommand>
    {
        public DailyOperationLoomDetailCommandValidator()
        {
            RuleFor(command => command.OrderDocumentId.Value).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.BeamOperatorId.Value).NotEmpty();
            RuleFor(command => command.BeamId.Value).NotEmpty();
            RuleFor(command => command.DailyOperationLoomHistory)
                .SetValidator(new DailyOperationLoomHistoryCommandValidator());
            RuleFor(command => command.WarpOrigin).NotEmpty();
            RuleFor(command => command.WeftOrigin).NotEmpty();
        }
    }
}
