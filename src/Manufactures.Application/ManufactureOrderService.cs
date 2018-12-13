using ExtCore.Data.Abstractions;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Threading.Tasks;
using Manufactures.Domain.Repositories;
using Manufactures.Domain;
using Manufactures.Domain.Entities;
using Manufactures.Domain.Events;
using Manufactures.Domain.ValueObjects;

namespace Manufactures.Application
{
    public class ManufactureOrderService : IManufactureOrderService
    {
        private readonly IStorage _storage;
        private readonly IManufactureOrderRepository _orderRepository;
        private readonly IWorkContext _workContext;

        public ManufactureOrderService(IStorage storage, IWorkContext workContext)
        {
            _storage = storage;

            _orderRepository = _storage.GetRepository<IManufactureOrderRepository>();

            _orderRepository.SetCurrentUser(workContext.CurrentUser);

            _workContext = workContext;
        }

        public async Task<ManufactureOrder> PlacedOrderAsync(DateTime date, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsConstruction construction, Blended blended, MachineId machineId)
        {
            var order = new ManufactureOrder(id: Guid.NewGuid(), 
                orderDate: date, 
                unitId: unitId, 
                yarnCodes: yarnCodes, 
                construction: construction, 
                blended: blended, 
                machineId: machineId,
                userId: _workContext.CurrentUser);

            await _orderRepository.Insert(order);

            _storage.Save();

            return order;
        }
    }
}
