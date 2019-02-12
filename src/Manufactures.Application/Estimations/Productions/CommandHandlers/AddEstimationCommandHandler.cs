using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Construction.Repositories;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.Productions.ValueObjects;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Orders.Repositories;
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
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;

        public AddEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimationProductRepository = _storage.GetRepository<IEstimationProductRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(AddNewEstimationCommand request, CancellationToken cancellationToken)
        {
            DateTimeOffset now = DateTimeOffset.Now;
            var year = now.Year.ToString();
            var month = now.Month.ToString();
            var estimationNumber = _estimationProductRepository.Find(o => o.Period.Deserialize<Period>().Year.Contains(year)).Count() + 1.ToString();
            estimationNumber = estimationNumber.PadLeft(4, '0') + "/" + month.PadLeft(2, '0') + "-" + year;

            var estimatedProductionDocument = new EstimatedProductionDocument(Guid.NewGuid(), estimationNumber, request.Period, request.Unit, request.TotalEstimationOrder);

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
                var newProduct = new EstimationProduct(Guid.NewGuid(), order, productGrade);

                estimatedProductionDocument.AddEstimationProduct(newProduct);
            }

            await _estimationProductRepository.Update(estimatedProductionDocument);
            _storage.Save();

            return estimatedProductionDocument;
        }
    }
}
