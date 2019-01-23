using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Rings.Commands
{
    public class RemoveRingDocumentCommand : ICommand<RingDocument>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }
    }
}
