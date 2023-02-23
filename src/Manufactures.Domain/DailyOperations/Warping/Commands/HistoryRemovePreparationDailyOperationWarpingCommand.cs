using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class HistoryRemovePreparationDailyOperationWarpingCommand : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "HistoryId")]
        public Guid HistoryId { get; set; }

        public void SetId(Guid id)
        {
            this.Id = id;
        }

        public void SetHistoryId(Guid historyId)
        {
            this.HistoryId = historyId;
        }
    }
    public class HistoryRemovePreparationDailyOperationWarpingCommandValidator : AbstractValidator<HistoryRemovePreparationDailyOperationWarpingCommand>
    {
        public HistoryRemovePreparationDailyOperationWarpingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty().WithMessage("Id Operasi Waroubg Tidak Boleh Kosong");
            RuleFor(validator => validator.HistoryId).NotEmpty().WithMessage("Id History Tidak Boleh Kosong");
        }
    }
}
