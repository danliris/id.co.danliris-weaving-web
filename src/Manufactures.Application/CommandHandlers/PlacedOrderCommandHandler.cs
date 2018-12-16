using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Domain.Commands
{
    public class PlaceOrderCommandHandler : ICommandHandler<PlaceOrderCommand, ManufactureOrder>
    {
        private readonly IManufactureOrderRepository _orderRepository;
        private readonly IStorage _storage;

        public PlaceOrderCommandHandler(IStorage storage)
        {
            _orderRepository = storage.GetRepository<IManufactureOrderRepository>();
            _storage = storage;
        }

        public async Task<ManufactureOrder> Handle(PlaceOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new ManufactureOrder(id: Guid.NewGuid(),
                orderDate: command.OrderDate,
                unitId: command.UnitDepartmentId,
                yarnCodes: command.YarnCodes,
                compositionId: command.CompositionId,
                blended: command.Blended,
                machineId: command.MachineId,
                userId: command.UserId);

            await _orderRepository.Update(order);

            _storage.Save();

            return order;
        }
    }
}