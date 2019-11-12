using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class UpdateCombFinishDailyOperationReachingCommand : ICommand<DailyOperationReachingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "CombWidth")]
        public double CombWidth { get; set; }

        [JsonProperty(PropertyName = "OperatorDocumentId")]
        public OperatorId OperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "YarnStrandsProcessed")]
        public int YarnStrandsProcessed { get; set; }

        [JsonProperty(PropertyName = "CombFinishDate")]
        public DateTimeOffset CombFinishDate { get; set; }

        [JsonProperty(PropertyName = "CombFinishTime")]
        public TimeSpan CombFinishTime { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateCombFinishDailyOperationReachingCommandValidator : AbstractValidator<UpdateCombFinishDailyOperationReachingCommand>
    {
        public UpdateCombFinishDailyOperationReachingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.CombWidth).NotEmpty();
            RuleFor(validator => validator.OperatorDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.YarnStrandsProcessed).NotEmpty();
            RuleFor(validator => validator.CombFinishDate).NotEmpty();
            RuleFor(validator => validator.CombFinishTime).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
        }
    }
}
