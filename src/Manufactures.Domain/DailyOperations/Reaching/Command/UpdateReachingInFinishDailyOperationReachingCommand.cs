using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class UpdateReachingInFinishDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ReachingInWidth")]
        public double ReachingInWidth { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "ReachingInFinishDate")]
        public DateTimeOffset ReachingInFinishDate { get; set; }

        [JsonProperty(PropertyName = "ReachingInFinishTime")]
        public TimeSpan ReachingInFinishTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateReachingInFinishDailyOperationReachingCommandValidator : AbstractValidator<UpdateReachingInFinishDailyOperationReachingCommand>
    {
        public UpdateReachingInFinishDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ReachingInWidth).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ReachingInFinishDate).NotEmpty();
            RuleFor(validator => validator.ReachingInFinishTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
