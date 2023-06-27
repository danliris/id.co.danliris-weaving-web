using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Productions;
using Manufactures.Domain.DailyOperations.Productions.Commands;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Production.CommandHandlers
{
    public class RemoveDailyOperationMachineSizingCommandHandler : ICommandHandler<RemoveDailyOperationMachineSizingCommand, DailyOperationMachineSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationMachineSizingDocumentRepository _dailyOperationMachineSizingDocumentRepository;
        private readonly IDailyOperationMachineSizingDetailRepository _dailyOperationMachineSizingDetailRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public RemoveDailyOperationMachineSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationMachineSizingDocumentRepository = _storage.GetRepository<IDailyOperationMachineSizingDocumentRepository>();
            _dailyOperationMachineSizingDetailRepository = _storage.GetRepository<IDailyOperationMachineSizingDetailRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<DailyOperationMachineSizingDocument> Handle(RemoveDailyOperationMachineSizingCommand request, CancellationToken cancellationToken)
        {
            var estimationDocument =
                _dailyOperationMachineSizingDocumentRepository
                    .Find(o => o.Identity == request.Id)
                    .FirstOrDefault();
            var estimationDetails =
                _dailyOperationMachineSizingDetailRepository
                    .Find(o => o.EstimatedProductionDocumentId == estimationDocument.Identity);

            if (estimationDocument == null)
            {
                Validator.ErrorValidation(("EstimationDocumentId", "Estimasi Produksi dengan Id " + request.Id + " Tidak Ditemukan"));
            }

            foreach (var estimationDetail in estimationDetails)
            {
                estimationDetail.SetDeleted();
                await _dailyOperationMachineSizingDetailRepository.Update(estimationDetail);

                var order =
                    _orderDocumentRepository
                        .Find(o => o.Identity == estimationDetail.OrderId.Value)
                        .FirstOrDefault();

                order.SetOrderStatus(Constants.ONORDER);
                await _orderDocumentRepository.Update(order);
            }

            estimationDocument.SetDeleted();
            await _dailyOperationMachineSizingDocumentRepository.Update(estimationDocument);

            _storage.Save();

            return estimationDocument;
        }
    }
}
