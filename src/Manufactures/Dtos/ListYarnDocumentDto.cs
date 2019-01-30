using Manufactures.Domain.Yarns;
using System;

namespace Manufactures.Dtos
{
    public class ListYarnDocumentDto
    {
        public ListYarnDocumentDto(YarnDocument document)
        {
            Id = document.Identity;
            Code = document.Code;
            Name = document.Name;
        }

        public Guid Id { get; }
        public string Code { get; }
        public string Name { get; }
    }
}
