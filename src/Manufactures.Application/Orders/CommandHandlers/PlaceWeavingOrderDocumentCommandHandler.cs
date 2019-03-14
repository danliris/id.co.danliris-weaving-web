using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
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
            // Get Order Number from auto generate
            var orderNumber = await _weavingOrderDocumentRepository.GetWeavingOrderNumber();

            //Set status
            var orderStatus = Constants.ONORDER;

            var order = new WeavingOrderDocument(id: Guid.NewGuid(),
                                                 orderNumber: orderNumber,
                                                 constructionId: new ConstructionId(Guid.Parse(command.FabricConstructionDocument.Id)),
                                                 dateOrdered: command.DateOrdered, 
                                                 period: command.Period,
                                                 warpComposition: command.WarpComposition,
                                                 weftComposition: command.WeftComposition,
                                                 warpOrigin: command.WarpOrigin,
                                                 weftOrigin: command.WeftOrigin, 
                                                 wholeGrade: command.WholeGrade,
                                                 yarnType: command.YarnType, 
                                                 unitId: new UnitId(command.WeavingUnit.Id),
                                                 orderStatus: orderStatus);
            
            await _weavingOrderDocumentRepository.Update(order);

            _storage.Save();

            return order;
        }
    }
}
