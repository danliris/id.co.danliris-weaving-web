using Infrastructure.Domain.Events;
using Manufactures.Domain.Orders;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Domain.Events
{
    public class OnManufactureOrderCreatedHandler : IDomainEventHandler<OnEntityCreated<ManufactureOrder>>
    {
        public Task Handle(OnEntityCreated<ManufactureOrder> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}