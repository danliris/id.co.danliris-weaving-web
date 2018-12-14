using Infrastructure.Domain.Events;
using Manufactures.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnManufactureOrderUpdatedHandler : IDomainEventHandler<OnEntityUpdated<ManufactureOrder>>
    {
        public Task Handle(OnEntityUpdated<ManufactureOrder> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
