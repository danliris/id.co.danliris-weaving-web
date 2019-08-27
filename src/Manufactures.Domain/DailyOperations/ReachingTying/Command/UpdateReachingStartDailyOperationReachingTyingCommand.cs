using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Command
{
    public class UpdateReachingStartDailyOperationReachingTyingCommand : ICommand<DailyOperationReachingTyingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ReachingTypeInput")]
        public string ReachingTypeInput { get; set; }

        [JsonProperty(PropertyName = "ReachingTypeOutput")]
        public string ReachingTypeOutput { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "ReachingStartDate")]
        public DateTimeOffset ReachingStartDate { get; set; }

        [JsonProperty(PropertyName = "ReachingStartTime")]
        public TimeSpan ReachingStartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateReachingStartDailyOperationReachingTyingCommandValidator : AbstractValidator<UpdateReachingStartDailyOperationReachingTyingCommand>
    {
        public UpdateReachingStartDailyOperationReachingTyingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ReachingTypeInput).NotEmpty();
            RuleFor(validator => validator.ReachingTypeOutput).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.ReachingStartDate).NotEmpty();
            RuleFor(validator => validator.ReachingStartTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
