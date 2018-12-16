using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Domain.Events
{
    public class OnManufactureOrderPlacedHandler : IManufactureEventHandler<OnManufactureOrderPlaced>
    {
        public Task Handle(OnManufactureOrderPlaced notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}