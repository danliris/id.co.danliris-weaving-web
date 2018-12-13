using ExtCore.Data.Abstractions;
using Moonlay.ExtCore.Mvc.Abstractions;
using System;
using System.Threading.Tasks;
using Weaving.Domain.Repositories;
using Weaving.Domain;
using Weaving.Domain.Entities;
using Weaving.Domain.Events;
using Weaving.Domain.ValueObjects;

namespace Weaving.Application
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

            // Pulish an event
            //new OnManufactureOroderPlaced(order.Identity).Broadcast();

            return order;
        }
    }
}
