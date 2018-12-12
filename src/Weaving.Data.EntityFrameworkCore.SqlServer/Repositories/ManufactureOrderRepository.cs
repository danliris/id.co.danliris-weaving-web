using System;
using System.Threading.Tasks;
using ExtCore.Data.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Weaving.Domain;
using Weaving.Domain.Entities;
using Weaving.Domain.ReadModels;

namespace Weaving.Application.Repositories
{
    public class ManufactureOrderRepository : RepositoryBase<ManufactureOrderReadModel>, IManufactureOrderRepository
    {
        public ManufactureOrderRepository()
        {
            GoodsConstructionSet = this.storageContext.Set<GoodsConstruction>();
        }

        public string CurrentUser { get; private set; }

        public DbSet<GoodsConstruction> GoodsConstructionSet { get; }

        public Task Update(ManufactureOrder order)
        {
            var readModel = new ManufactureOrderReadModel(order.Identity)
            {
                MachineId = order.MachineId.Value,
                UnitDepartmentId = order.UnitId.Value,
                YarnCodesJson = order.YarnCodes.ToString(),
                State = order.State,
                OrderDate = order.OrderDate,
                BlendedJson = order.Blended.ToString(),

                //CreatedBy = CurrentUser,
                //Deleted = false
            };

            dbSet.Update(readModel);

            return Task.CompletedTask;
        }

        public void SetCurrentUser(string userId)
        {
            CurrentUser = userId;
        }
    }
}
