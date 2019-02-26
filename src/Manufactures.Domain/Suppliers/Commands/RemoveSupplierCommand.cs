using Infrastructure.Domain.Commands;
using System;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class RemoveSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        public void SetId(Guid Id) { this.Id = Id; }
        public Guid Id { get; private set; }
    }
}
