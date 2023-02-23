using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class UpdateCombStartDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "CombStartDate")]
        public DateTimeOffset CombStartDate { get; set; }

        [JsonProperty(PropertyName = "CombStartTime")]
        public TimeSpan CombStartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "CombEdgeStitching")]
        public int CombEdgeStitching { get; set; }

        [JsonProperty(PropertyName = "CombNumber")]
        public int CombNumber { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateCombStartDailyOperationReachingCommandValidator : AbstractValidator<UpdateCombStartDailyOperationReachingCommand>
    {
        public UpdateCombStartDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.CombStartDate).NotEmpty();
            RuleFor(validator => validator.CombStartTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.CombEdgeStitching).NotEmpty();
            RuleFor(validator => validator.CombNumber).NotEmpty();
        }
    }
}
