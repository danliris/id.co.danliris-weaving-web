using System;
using System.Threading.Tasks;
using Weaving.Domain;
using Weaving.Domain.Entities;
using Weaving.Domain.ValueObjects;

namespace Weaving.Application
{
    public interface IManufactureOrderService
    {
        Task<ManufactureOrder> PlacedOrderAsync(DateTime date, UnitDepartmentId unitId, YarnCodes yarnCodes, GoodsConstruction construction, Blended blended, MachineId machineId);
    }
}