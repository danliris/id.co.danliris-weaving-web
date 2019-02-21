using Manufactures.Domain.Rings;
using System;

namespace Manufactures.Dtos
{
    public class RingDocumentDto
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public int Number { get; private set; }
        public string RingType { get; private set; }
        public string Description { get; private set; }

        public RingDocumentDto(RingDocument ringDocument)
        {
            Id = ringDocument.Identity;
            Code = ringDocument.Code;
            Number = ringDocument.Number;
            RingType = ringDocument.RingType;
            Description = ringDocument.Description;
        }
    }
}
