using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class PreparationDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "LoomBeamHistories")]
        public List<DailyOperationLoomBeamHistoryCommand> LoomBeamHistories { get; set; }
    }

    public class PreparationDailyOperationLoomCommandValidator : AbstractValidator<PreparationDailyOperationLoomCommand>
    {
        public PreparationDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.OrderDocumentId).NotEmpty().WithMessage("No. Order Produksi Harus Diisi");
            RuleFor(validator => validator.LoomBeamHistories).Must(list => list.Count > 0).WithMessage("Beam Sizing Yang Diproses Tidak Boleh Kosong");
            RuleForEach(validator => validator.LoomBeamHistories).SetValidator(new DailyOperationLoomBeamHistoryCommandValidator());
        }
    }
}
