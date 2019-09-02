using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Command
{
    public class ChangeOperatorReachingDailyOperationReachingTyingCommand : ICommand<DailyOperationReachingTyingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorReachingDate")]
        public DateTimeOffset ChangeOperatorReachingDate { get; set; }

        [JsonProperty(PropertyName = "ChangeOperatorReachingTime")]
        public TimeSpan ChangeOperatorReachingTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ChangeOperatorReachingDailyOperationReachingTyingCommandValidator : AbstractValidator<ChangeOperatorReachingDailyOperationReachingTyingCommand>
    {
        public ChangeOperatorReachingDailyOperationReachingTyingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.ChangeOperatorReachingDate).NotEmpty();
            RuleFor(validator => validator.ChangeOperatorReachingTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
