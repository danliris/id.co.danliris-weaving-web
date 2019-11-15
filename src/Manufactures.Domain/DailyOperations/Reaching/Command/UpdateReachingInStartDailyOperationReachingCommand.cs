using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class UpdateReachingInStartDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ReachingInTypeInput")]
        public string ReachingInTypeInput { get; set; }

        [JsonProperty(PropertyName = "ReachingInTypeOutput")]
        public string ReachingInTypeOutput { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "ReachingInStartDate")]
        public DateTimeOffset ReachingInStartDate { get; set; }

        [JsonProperty(PropertyName = "ReachingInStartTime")]
        public TimeSpan ReachingInStartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateReachingInStartDailyOperationReachingCommandValidator : AbstractValidator<UpdateReachingInStartDailyOperationReachingCommand>
    {
        public UpdateReachingInStartDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ReachingInTypeInput).NotEmpty();
            RuleFor(validator => validator.ReachingInTypeOutput).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.ReachingInStartDate).NotEmpty();
            RuleFor(validator => validator.ReachingInStartTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
