using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Rings.Commands
{
    public class RemoveRingDocumentCommand : ICommand<RingDocument>
    {
        public Guid Id { get; private set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
