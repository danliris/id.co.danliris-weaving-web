using System.Threading;
using System.Threading.Tasks;
using Weaving.Domain.Events;

namespace Weaving.Application.EventHandlers
{
    public class OnWeavingOrderPlacedHandler : IWeavingEventHandler<OnWeavingOrderPlaced>
    {
        public Task Handle(OnWeavingOrderPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
