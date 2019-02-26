using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Yarns.Commands
{

    public class RemoveExsistingYarnCommand : ICommand<YarnDocument>
    {
        public void SetId(Guid Id) { this.Id = Id; }
        public Guid Id { get; private set; }
    }
}
