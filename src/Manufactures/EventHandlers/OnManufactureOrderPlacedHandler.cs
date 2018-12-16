using Manufactures.Domain.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.EventHandlers
{
    public class OnManufactureOrderPlacedHandler : IManufactureEventHandler<OnManufactureOrderPlaced>
    {
        public Task Handle(OnManufactureOrderPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}