using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.CommandHandlers
{
    public class UpdateWeavingOrderDocumentCommandHandler : ICommandHandler<UpdateWeavingOrderCommand, WeavingOrderDocument>
    {
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IStorage _storage;

        public UpdateWeavingOrderDocumentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }


        public async Task<WeavingOrderDocument> Handle(UpdateWeavingOrderCommand command, CancellationToken cancellationToken)
        {
            var order = _weavingOrderDocumentRepository.Find(entity => entity.Identity == command.Id).FirstOrDefault();

            if(order == null)
            {
                throw Validator.ErrorValidation(("Id", "Invalid Order: " + command.Id));
            }

            order.SetFabricConstructionDocument(command.FabricConstructionDocument);
            order.SetWarpOrigin(command.WarpOrigin);
            order.SetWeftOrigin(command.WeftOrigin);
            order.SetWholeGrade(command.WholeGrade);
            order.SetYarnType(command.YarnType);
            order.SetPeriod(command.Period);
            order.SetComposition(command.Composition);
            order.SetWeavingUnit(command.WeavingUnit);
            order.SetUserId(command.UserId);

            await _weavingOrderDocumentRepository.Update(order);

            _storage.Save();
            
            return order;
        }
    }
}
