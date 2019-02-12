using Manufactures.Domain.GlobalValueObjects;
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
            Tags = document.Tags;
            MaterialTypeDocument = document.MaterialTypeDocument;
            RingDocument = document.RingDocument;
        }

        public Guid Id { get; }
        public string Code { get; }
        public string Name { get; }
        public string Tags { get; }
        public MaterialTypeValueObject MaterialTypeDocument { get; }
        public RingDocumentValueObject RingDocument { get; }
    }
}
