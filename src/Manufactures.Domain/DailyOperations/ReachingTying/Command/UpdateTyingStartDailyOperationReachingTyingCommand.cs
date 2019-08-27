using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Command
{
    public class UpdateTyingStartDailyOperationReachingTyingCommand : ICommand<DailyOperationReachingTyingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "TyingEdgeStitching")]
        public int TyingEdgeStitching { get; set; }

        [JsonProperty(PropertyName = "TyingNumber")]
        public int TyingNumber { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "TyingStartDate")]
        public DateTimeOffset TyingStartDate { get; set; }

        [JsonProperty(PropertyName = "TyingStartTime")]
        public TimeSpan TyingStartTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateTyingStartDailyOperationReachingTyingCommandValidator : AbstractValidator<UpdateTyingStartDailyOperationReachingTyingCommand>
    {
        public UpdateTyingStartDailyOperationReachingTyingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.TyingEdgeStitching).NotEmpty();
            RuleFor(validator => validator.TyingNumber).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.TyingStartDate).NotEmpty();
            RuleFor(validator => validator.TyingStartTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
