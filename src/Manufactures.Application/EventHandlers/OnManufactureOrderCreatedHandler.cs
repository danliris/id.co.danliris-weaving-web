using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Domain.Events;
using Manufactures.Domain;

namespace Manufactures.Application.EventHandlers
{
    public class OnManufactureOrderCreatedHandler : IDomainEventHandler<OnEntityCreated<ManufactureOrder>>
    {
        public Task Handle(OnEntityCreated<ManufactureOrder> notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
