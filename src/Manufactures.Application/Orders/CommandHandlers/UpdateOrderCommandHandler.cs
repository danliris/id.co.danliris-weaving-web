using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Domain.Orders.Commands
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, ManufactureOrder>
    {
        private readonly IManufactureOrderRepository _manufactureOrderRepo;

        public UpdateOrderCommandHandler(IStorage storage)
        {
            Storage = storage;
            _manufactureOrderRepo = Storage.GetRepository<IManufactureOrderRepository>();
        }

        private IStorage Storage { get; }


        public async Task<ManufactureOrder> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _manufactureOrderRepo.Find(o => o.Identity == request.Id).FirstOrDefault();

            if (order == null)
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + request.Id));

            order.SetBlended(request.Blended);
            order.SetMachineId(request.MachineId);
            order.SetUnitDepartment(request.UnitDepartmentId);
            order.SetUserId(request.UserId);
            order.SetYarnCodes(request.YarnCodes);

            await _manufactureOrderRepo.Update(order);

            // Save Changes
            Storage.Save();

            return order;
        }
    }
}
