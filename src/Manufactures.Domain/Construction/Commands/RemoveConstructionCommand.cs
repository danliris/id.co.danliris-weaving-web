using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Construction.Commands
{
    public class RemoveConstructionCommand : ICommand<ConstructionDocument>
    {
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
