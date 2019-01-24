using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Materials.Commands
{
    public class RemoveMaterialTypeCommand : ICommand<MaterialTypeDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
