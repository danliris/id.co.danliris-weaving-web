using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.ValueObjects;
using System;

namespace Manufactures.Dtos
{
    public class YarnDocumentDto
    {
        public YarnDocumentDto(YarnDocument document)
        {
            Id = document.Identity;
            Code = document.Code;
            Name = document.Name;
            Description = document.Description;
            Tags = document.Tags;
            CoreCurrency = document.CoreCurrency;
            CoreUom = document.CoreUom;
            MaterialTypeDocument = document.MaterialTypeDocument;
            RingDocument = document.RingDocument;
            Price = document.Price;
        }

        public Guid Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Description { get; }
        public string Tags { get; }
        public CurrencyValueObject CoreCurrency { get; }
        public UomValueObject CoreUom { get; }
        public MaterialTypeDocumentValueObject MaterialTypeDocument { get; }
        public RingDocumentValueObject RingDocument { get; }
        public double Price { get; }
    }
}
