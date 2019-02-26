using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Materials.Commands
{
    public class RemoveMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        public void SetId(Guid Id) { this.Id = Id; }
        public Guid Id { get; private set; }
    }
}
