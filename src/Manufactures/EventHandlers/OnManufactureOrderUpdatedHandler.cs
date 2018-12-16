using Infrastructure.Domain.Events;
using Manufactures.Domain.Orders;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Domain.Events
{
    public class OnManufactureOrderUpdatedHandler : IDomainEventHandler<OnEntityUpdated<ManufactureOrder>>
    {
        public Task Handle(OnEntityUpdated<ManufactureOrder> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}