using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class AddOrderCommand : ICommand<OrderDocument>
    {
        [JsonProperty(PropertyName = "Year")]
        public int Year { get; private set; }

        [JsonProperty(PropertyName = "Month")]
        public int Month { get; private set; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; private set; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; private set; }

        [JsonProperty(PropertyName = "WarpOriginId")]
        public SupplierId WarpOriginId { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionPoly")]
        public double WarpCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionCotton")]
        public double WarpCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionOthers")]
        public double WarpCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "WeftOriginId")]
        public SupplierId WeftOriginId { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionPoly")]
        public double WeftCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionCotton")]
        public double WeftCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionOthers")]
        public double WeftCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "AllGrade")]
        public double AllGrade { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; private set; }
    }

    public class AddOrderCommandValidator : AbstractValidator<AddOrderCommand>
    {
        public AddOrderCommandValidator()
        {
            RuleFor(command => command.Year).NotEmpty().WithMessage("Tahun Harus Diisi");
            RuleFor(command => command.Month).NotEmpty().WithMessage("Bulan Harus Diisi");
            RuleFor(command => command.ConstructionDocumentId).NotEmpty().WithMessage("Konstruksi Harus Diisi");
            RuleFor(command => command.YarnType).NotEmpty().WithMessage("Jenis Benang Tidak Boleh Kosong");
            RuleFor(command => command.WarpOriginId).NotEmpty().WithMessage("Asal Lusi Harus Diisi");
            RuleFor(command => command.WarpCompositionPoly).NotEmpty().WithMessage("Poly Harus Diisi");
            RuleFor(command => command.WarpCompositionCotton).NotEmpty().WithMessage("Cotton Harus Diisi");
            RuleFor(command => command.WeftOriginId).NotEmpty().WithMessage("Asal Pakan Harus Diisi");
            RuleFor(command => command.WeftCompositionPoly).NotEmpty().WithMessage("Poly Harus Diisi");
            RuleFor(command => command.WeftCompositionCotton).NotEmpty().WithMessage("Cotton Harus Diisi");
            RuleFor(command => command.AllGrade).NotEmpty().WithMessage("All Grade Harus Diisi");
            RuleFor(command => command.UnitId).NotEmpty().WithMessage("Unit Harus Diisi");
        }
    }
}
