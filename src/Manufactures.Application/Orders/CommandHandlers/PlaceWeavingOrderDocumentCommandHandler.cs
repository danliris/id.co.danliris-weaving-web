using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
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

        public async Task<WeavingOrderDocument> Handle(PlaceWeavingOrderCommand command, CancellationToken cancellationToken)
        {
            var order = new WeavingOrderDocument(id: Guid.NewGuid(),
                orderNumber: command.OrderNumber,
                fabricConstruction: command.FabricConstruction,
                dateOrdered: command.DateOrdered, period: command.Period,
                composition: command.Composition, warpOrigin: command.WarpOrigin,
                weftOrigin: command.WeftOrigin, wholeGrade: command.WholeGrade,
                yarnType: command.YarnType, weavingUnit: command.WeavingUnit,
                userId: command.UserId);

            await _weavingOrderDocumentRepository.Update(order);

            _storage.Save();

            return order;
        }
    }
}
