using Infrastructure.Domain;
using Manufactures.Domain.MachineTypes.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.MachineTypes
{
    public class MachineTypeDocument : AggregateRoot<MachineTypeDocument, MachineTypeReadModel>
    {

        public MachineTypeDocument(Guid id): base(id)
        {
            Identity = id;
        }
        
        public MachineTypeDocument(MachineTypeReadModel readModel) : base(readModel)
        {

        }

        protected override MachineTypeDocument GetEntity()
        {
            return this;
        }
    }
}
