using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class PreparationDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "OrderDocumentId")]
        public OrderId OrderDocumentId { get; set; }

        [JsonProperty(PropertyName = "BeamProcessed")]
        public int BeamProcessed { get; set; }

        [JsonProperty(PropertyName = "LoomItems")]
        public List<PreparationDailyOperationLoomItemsCommand> LoomItems { get; set; }
    }

    public class PreparationDailyOperationLoomCommandValidator : AbstractValidator<PreparationDailyOperationLoomCommand>
    {
        public PreparationDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.OrderDocumentId).NotEmpty().WithMessage("No. Order Produksi Harus Diisi");
            RuleFor(validator => validator.BeamProcessed).NotEmpty().WithMessage("Jumlah Beam Diproses Harus Diisi");
            RuleFor(validator => validator.LoomItems).Must(list => list.Count > 0).WithMessage("Beam Diproses Tidak Boleh Kosong");
            RuleForEach(validator => validator.LoomItems).SetValidator(new PreparationDailyOperationLoomItemsCommandValidator());
        }
    }
}
