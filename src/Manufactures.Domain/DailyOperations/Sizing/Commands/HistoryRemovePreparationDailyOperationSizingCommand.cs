using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class HistoryRemovePreparationDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
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

    public class HistoryRemovePreparationDailyOperationSizingCommandValidator : AbstractValidator<HistoryRemovePreparationDailyOperationSizingCommand>
    {
        public HistoryRemovePreparationDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty().WithMessage("Id Operasi Sizing Tidak Boleh Kosong");
            RuleFor(validator => validator.HistoryId).NotEmpty().WithMessage("Id History Tidak Boleh Kosong");
        }
    }
}
