using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class ChangeOperatorReachingInDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorReachingInDate")]
        public DateTimeOffset ChangeOperatorReachingInDate { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorReachingInTime")]
        public TimeSpan ChangeOperatorReachingInTime { get; set; }

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

    public class ChangeOperatorReachingInDailyOperationReachingCommandValidator : AbstractValidator<ChangeOperatorReachingInDailyOperationReachingCommand>
    {
        public ChangeOperatorReachingInDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ChangeOperatorReachingInDate).NotEmpty();
            RuleFor(validator => validator.ChangeOperatorReachingInTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
        }
    }
}
