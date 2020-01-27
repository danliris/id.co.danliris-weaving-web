using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.CommandHandlers
{
    public class RemoveWeavingOrderCommandHandler : ICommandHandler<RemoveOrderCommand, OrderDocument>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IStorage _storage;

        public RemoveWeavingOrderCommandHandler(IStorage storage)
        {
            _storage = storage;
            _orderRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<OrderDocument> Handle(RemoveOrderCommand command, 
                                                       CancellationToken cancellationToken)
        {
            var order = _orderRepository.Find(entity => entity.Identity == command.Id)
                                                       .FirstOrDefault();

            if (order == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + command.Id));
            }

            order.Remove();

            await _orderRepository.Update(order);

            _storage.Save();

            return order;
        }
    }
}
