using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Estimations.Productions.CommandHandlers
{
    public class RemoveEstimationCommandHandler : ICommandHandler<RemoveEstimationProductCommand, EstimatedProductionDocument>
    {
        private readonly IStorage _storage;
        private readonly IEstimationProductRepository _estimationProductRepository;
        private readonly IOrderRepository _weavingOrderDocumentRepository;

        public RemoveEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimationProductRepository = _storage.GetRepository<IEstimationProductRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(RemoveEstimationProductCommand request, CancellationToken cancellationToken)
        {
            var query = _estimationProductRepository.Query.Include(o => o.EstimationProducts);
            var exsistingEstimation = _estimationProductRepository.Find(query).Where(entity => entity.Identity.Equals(request.Id)).FirstOrDefault();

            if (exsistingEstimation == null)
            {
                Validator.ErrorValidation(("Estimation Document", "Unavailable exsisting Estimation Document with Id " + request.Id));
            }

            foreach(var estimatedProduct in exsistingEstimation.EstimationProducts)
            {
                var order = _weavingOrderDocumentRepository.Find(e => e.OrderNumber.Equals(estimatedProduct.OrderDocument
                                                                                                           .Deserialize<EstimationProductValueObject>()
                                                                                                           .OrderNumber))
                                                           .FirstOrDefault();

                order.SetOrderStatus(Constants.ONORDER);
                await _weavingOrderDocumentRepository.Update(order);
            }

            exsistingEstimation.Remove();
            await _estimationProductRepository.Update(exsistingEstimation);
            _storage.Save();

            return exsistingEstimation;
        }
    }
}
