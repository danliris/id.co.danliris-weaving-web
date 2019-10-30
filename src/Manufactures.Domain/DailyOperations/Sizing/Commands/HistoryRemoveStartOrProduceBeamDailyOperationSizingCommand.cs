using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "HistoryId")]
        public Guid HistoryId { get; set; }

        [JsonProperty(PropertyName = "BeamProductId")]
        public Guid BeamProductId { get; set; }

        [JsonProperty(PropertyName = "HistoryStatus")]
        public string HistoryStatus { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandValidator : AbstractValidator<HistoryRemoveStartOrProduceBeamDailyOperationSizingCommand>
    {
        public HistoryRemoveStartOrProduceBeamDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty().WithMessage("Id Operasi Sizing Tidak Boleh Kosong");
            RuleFor(validator => validator.HistoryId).NotEmpty().WithMessage("Id History Tidak Boleh Kosong");
            RuleFor(validator => validator.BeamProductId).NotEmpty().WithMessage("Id Produk Beam Tidak Boleh Kosong");
            RuleFor(validator => validator.HistoryStatus).NotEmpty().WithMessage("Status History Tidak Boleh Kosong");
        }
    }
}
