using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class UpdateOrderCommand : ICommand<OrderDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Period")]
        public DateTimeOffset Period { get; private set; }

        [JsonProperty(PropertyName = "ConstructionDocumentId")]
        public ConstructionId ConstructionDocumentId { get; private set; }

        [JsonProperty(PropertyName = "YarnType")]
        public string YarnType { get; private set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public SupplierId WarpOrigin { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionPoly")]
        public double WarpCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionCotton")]
        public double WarpCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WarpCompositionOthers")]
        public double WarpCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public SupplierId WeftOrigin { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionPoly")]
        public double WeftCompositionPoly { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionCotton")]
        public double WeftCompositionCotton { get; private set; }

        [JsonProperty(PropertyName = "WeftCompositionOthers")]
        public double WeftCompositionOthers { get; private set; }

        [JsonProperty(PropertyName = "AllGrade")]
        public double AllGrade { get; private set; }

        [JsonProperty(PropertyName = "Unit")]
        public UnitId Unit { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateWeavingOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateWeavingOrderCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty().WithMessage("Id Harus Valid");
            RuleFor(command => command.Period).NotEmpty().WithMessage("Periode Harus Diisi");
            RuleFor(command => command.ConstructionDocumentId).NotEmpty().WithMessage("Konstruksi Harus Diisi");
            RuleFor(command => command.YarnType).NotEmpty().WithMessage("Jenis Benang Tidak Boleh Kosong");
            RuleFor(command => command.WarpOrigin).NotEmpty().WithMessage("Asal Lusi Harus Diisi");
            RuleFor(command => command.WarpCompositionPoly).NotEmpty().WithMessage("Poly Harus Diisi");
            RuleFor(command => command.WarpCompositionCotton).NotEmpty().WithMessage("Cotton Harus Diisi");
            RuleFor(command => command.WeftOrigin).NotEmpty().WithMessage("Asal Pakan Harus Diisi");
            RuleFor(command => command.WeftCompositionPoly).NotEmpty().WithMessage("Poly Harus Diisi");
            RuleFor(command => command.WeftCompositionCotton).NotEmpty().WithMessage("Cotton Harus Diisi");
            RuleFor(command => command.AllGrade).NotEmpty().WithMessage("All Grade Harus Diisi");
            RuleFor(command => command.Unit).NotEmpty().WithMessage("Unit Harus Diisi");
        }
    }
}
