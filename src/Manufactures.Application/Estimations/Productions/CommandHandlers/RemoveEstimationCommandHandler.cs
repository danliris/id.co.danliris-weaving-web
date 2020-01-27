using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Estimations.Productions.CommandHandlers
{
    public class RemoveEstimationCommandHandler : ICommandHandler<RemoveEstimationProductCommand, EstimatedProductionDocument>
    {
        private readonly IStorage _storage;
        private readonly IEstimatedProductionDocumentRepository _estimatedProductionDocumentRepository;
        private readonly IEstimatedProductionDetailRepository _estimatedProductionDetailRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public RemoveEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimatedProductionDocumentRepository = _storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _estimatedProductionDetailRepository = _storage.GetRepository<IEstimatedProductionDetailRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(RemoveEstimationProductCommand request, CancellationToken cancellationToken)
        {
            var estimationDocument =
                _estimatedProductionDocumentRepository
                    .Find(o => o.Identity == request.Id)
                    .FirstOrDefault();
            var estimationDetails =
                _estimatedProductionDetailRepository
                    .Find(o => o.EstimatedProductionDocumentId == estimationDocument.Identity);

            if (estimationDocument == null)
            {
                Validator.ErrorValidation(("EstimationDocumentId", "Estimasi Produksi dengan Id " + request.Id + " Tidak Ditemukan"));
            }

            foreach (var estimationDetail in estimationDetails)
            {
                estimationDetail.SetDeleted();
                await _estimatedProductionDetailRepository.Update(estimationDetail);

                var order =
                    _orderDocumentRepository
                        .Find(o => o.Identity == estimationDetail.OrderId.Value)
                        .FirstOrDefault();

                order.SetOrderStatus(Constants.ONORDER);
                await _orderDocumentRepository.Update(order);
            }

            estimationDocument.SetDeleted();
            await _estimatedProductionDocumentRepository.Update(estimationDocument);

            _storage.Save();

            return estimationDocument;
        }
    }
}
