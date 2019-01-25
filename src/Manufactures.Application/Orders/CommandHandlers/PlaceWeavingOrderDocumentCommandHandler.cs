using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Orders.CommandHandlers
{
    public class PlaceWeavingOrderDocumentCommandHandler : ICommandHandler<PlaceWeavingOrderCommand, WeavingOrderDocument>
    {
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;
        private readonly IStorage _storage;

        public PlaceWeavingOrderDocumentCommandHandler(IStorage storage)
        {
            _storage = storage;
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<WeavingOrderDocument> Handle(PlaceWeavingOrderCommand command, 
                                                       CancellationToken cancellationToken)
        {
            // Checking for same order number
            var isHasSameOrderNumber = _weavingOrderDocumentRepository.Find(entity => entity.OrderNumber.Equals(command.OrderNumber)).Count() >= 1;

            if (isHasSameOrderNumber)
            {
                command.OrderNumber = await _weavingOrderDocumentRepository.GetWeavingOrderNumber();
            }
            
            var order = new WeavingOrderDocument(id: Guid.NewGuid(),
                                                 orderNumber: command.OrderNumber,
                                                 fabricConstructionDocument: command.FabricConstructionDocument,
                                                 dateOrdered: command.DateOrdered, 
                                                 period: command.Period,
                                                 composition: command.Composition, 
                                                 warpOrigin: command.WarpOrigin,
                                                 weftOrigin: command.WeftOrigin, 
                                                 wholeGrade: command.WholeGrade,
                                                 yarnType: command.YarnType, 
                                                 weavingUnit: command.WeavingUnit);
            
            await _weavingOrderDocumentRepository.Update(order);

            _storage.Save();

            return order;
        }
    }
}
