using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class ChangeOperatorCombDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorCombDate")]
        public DateTimeOffset CombFinishDate { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorCombTime")]
        public TimeSpan CombFinishTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ChangeOperatorCombDailyOperationReachingCommandValidator : AbstractValidator<ChangeOperatorCombDailyOperationReachingCommand>
    {
        public ChangeOperatorCombDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.CombFinishDate).NotEmpty();
            RuleFor(validator => validator.CombFinishTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
        }
    }
}
