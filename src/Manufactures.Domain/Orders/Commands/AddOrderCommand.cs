using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class AddOrderCommand : ICommand<OrderDocument>
    {
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
        //-------------------------------------------------------------------------
        //[JsonProperty(PropertyName = "FabricConstructionDocument")]
        //public FabricConstructionCommand FabricConstructionDocument { get; set; }

        //[JsonProperty(PropertyName = "DateOrdered")]
        //public DateTimeOffset DateOrdered { get; set; }

        //[JsonProperty(PropertyName = "WarpOriginId")]
        //public string WarpOriginId { get; set; }

        //[JsonProperty(PropertyName = "WeftOriginId")]
        //public string WeftOriginId { get; set; }

        //[JsonProperty(PropertyName = "WholeGrade")]
        //public int WholeGrade { get; set; }

        //[JsonProperty(PropertyName = "YarnType")]
        //public string YarnType { get; set; }

        //[JsonProperty(PropertyName = "Period")]
        //public Period Period { get; set; }

        //[JsonProperty(PropertyName = "WarpComposition")]
        //public Composition WarpComposition { get; set; }

        //[JsonProperty(PropertyName = "WeftComposition")]
        //public Composition WeftComposition { get; set; }

        //[JsonProperty(PropertyName = "WeavingUnit")]
        //public WeavingUnit WeavingUnit { get; set; }
    }

    public class WeavingOrderCommandValidator : AbstractValidator<AddOrderCommand>
    {
        public WeavingOrderCommandValidator()
        {
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
