using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ReachingTying.Command
{
    public class UpdateTyingFinishDailyOperationReachingTyingCommand : ICommand<DailyOperationReachingTyingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "TyingWidth")]
        public double TyingWidth { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "TyingFinishDate")]
        public DateTimeOffset TyingFinishDate { get; set; }

        [JsonProperty(PropertyName = "TyingFinishTime")]
        public TimeSpan TyingFinishTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateTyingFinishDailyOperationReachingTyingCommandValidator : AbstractValidator<UpdateTyingFinishDailyOperationReachingTyingCommand>
    {
        public UpdateTyingFinishDailyOperationReachingTyingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.TyingWidth).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.TyingFinishDate).NotEmpty();
            RuleFor(validator => validator.TyingFinishTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
