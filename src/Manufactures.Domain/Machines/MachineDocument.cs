using Infrastructure.Domain;
using Manufactures.Domain.Machines.ReadModels;
using System;

namespace Manufactures.Domain.Machines
{
    public class MachineDocument : AggregateRoot<MachineDocument, MachineDocumentReadModel>
    {
        public MachineDocument(Guid identity) : base(identity)
        {
            Identity = identity;
        }

        protected override MachineDocument GetEntity()
        {
            return this;
        }
    }
}
