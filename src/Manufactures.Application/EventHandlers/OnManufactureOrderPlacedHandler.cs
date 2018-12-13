using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Events;

namespace Manufactures.Application.EventHandlers
{
    public class OnManufactureOrderPlacedHandler : IManufactureEventHandler<OnManufactureOrderPlaced>
    {
        public Task Handle(OnManufactureOrderPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
