using Manufactures.Domain.Materials;
using System;

namespace Manufactures.Dtos
{
    public class MaterialTypeDto
    {
        public MaterialTypeDto(MaterialType materialType)
        {
            Id = materialType.Identity;
            Code = materialType.Code;
            Name = materialType.Name;
         }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
    }
}
