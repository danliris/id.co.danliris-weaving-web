using System;
using System.Threading.Tasks;
using Manufactures.Domain;
using Manufactures.Domain.Entities;
using Manufactures.Domain.ValueObjects;

namespace Manufactures.Application
{
    public interface IManufactureOrderService
    {
        Task<ManufactureOrder> PlacedOrderAsync(DateTime date, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsComposition construction, Blended blended, MachineId machineId);
    }
}