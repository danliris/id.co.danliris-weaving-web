using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Manufactures.Dtos
{
    public class MaterialTypeDto
    {

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public List<RingDocumentValueObject> RingDocuments { get; private set; }
        public string Description { get; private set; }

        public MaterialTypeDto(MaterialTypeDocument materialType)
        {
            Id = materialType.Identity;
            Code = materialType.Code;
            Name = materialType.Name;
            RingDocuments = materialType.RingDocuments.ToList();
            Description = materialType.Description;
         }

    }
}
