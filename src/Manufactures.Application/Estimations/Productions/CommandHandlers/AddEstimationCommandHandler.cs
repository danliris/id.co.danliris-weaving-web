using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.Estimations.Productions.CommandHandlers
{
    public class AddEstimationCommandHandler : ICommandHandler<AddNewEstimationCommand, EstimatedProductionDocument>
    {
        private readonly IStorage _storage;
        private readonly IEstimationProductRepository _estimationProductRepository;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;

        public AddEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimationProductRepository = _storage.GetRepository<IEstimationProductRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IFabricConstructionRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(AddNewEstimationCommand request, CancellationToken cancellationToken)
        {
            var estimationNumber = await _estimationProductRepository.GetEstimationNumber();

            var estimatedProductionDocument = new EstimatedProductionDocument(Guid.NewGuid(), estimationNumber, request.Period, new UnitId(request.Unit.Id));

            foreach(var product in request.EstimationProducts)
            {
                var exsistingConstructionDocument = _constructionDocumentRepository.Find(o => o.ConstructionNumber.Equals(product.ConstructionNumber)).FirstOrDefault();
                var existingOrder = _weavingOrderDocumentRepository.Find(o => o.OrderNumber.Equals(product.OrderNumber)).FirstOrDefault();

                var constructionDocument = new ConstructionDocument(exsistingConstructionDocument.Identity, 
                                                            exsistingConstructionDocument.ConstructionNumber,
                                                            exsistingConstructionDocument.TotalYarn);
                var productGrade = new ProductGrade(product.GradeA, product.GradeB, product.GradeC, product.GradeD);
                var order = new OrderDocumentValueObject(existingOrder.Identity, 
                                                         existingOrder.OrderNumber, 
                                                         existingOrder.WholeGrade, 
                                                         constructionDocument,
                                                         existingOrder.DateOrdered);
                var newProduct = new EstimationProduct(Guid.NewGuid(), order, productGrade, product.TotalGramEstimation);

                estimatedProductionDocument.AddEstimationProduct(newProduct);
                existingOrder.SetOrderStatus(Constants.ONESTIMATED);
                await _weavingOrderDocumentRepository.Update(existingOrder);
            }

            await _estimationProductRepository.Update(estimatedProductionDocument);
           
            _storage.Save();

            return estimatedProductionDocument;
        }
    }
}
