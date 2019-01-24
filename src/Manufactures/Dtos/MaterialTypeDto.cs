using Manufactures.Domain.Materials;
using System;

namespace Manufactures.Dtos
{
    public class MaterialTypeDto
    {
        public MaterialTypeDto(MaterialTypeDocument materialType)
        {
            Id = materialType.Identity;
            Code = materialType.Code;
            Name = materialType.Name;
            Description = materialType.Description;
         }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
    }
}
