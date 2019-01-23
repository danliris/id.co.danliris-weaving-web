using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Construction.Commands
{
    public class RemoveConstructionCommand : ICommand<ConstructionDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
