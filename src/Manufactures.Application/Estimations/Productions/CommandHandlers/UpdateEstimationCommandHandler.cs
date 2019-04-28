using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstruction.Repositories;
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
    public class UpdateEstimationCommandHandler : ICommandHandler<UpdateEstimationProductCommand, EstimatedProductionDocument>
    {
        private readonly IStorage _storage;
        private readonly IEstimationProductRepository _estimationProductRepository;
        private readonly IConstructionDocumentRepository _constructionDocumentRepository;
        private readonly IWeavingOrderDocumentRepository _weavingOrderDocumentRepository;

        public UpdateEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimationProductRepository = _storage.GetRepository<IEstimationProductRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IConstructionDocumentRepository>();
            _weavingOrderDocumentRepository = _storage.GetRepository<IWeavingOrderDocumentRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(UpdateEstimationProductCommand request, CancellationToken cancellationToken)
        {
            var query = _estimationProductRepository.Query.Include(o => o.EstimationProducts);
            var exsistingEstimation = _estimationProductRepository.Find(query).Where(entity => entity.Identity.Equals(request.Id)).FirstOrDefault();

            if(exsistingEstimation == null)
            {
                Validator.ErrorValidation(("Estimation Document", "Unavailable exsisting Estimation Document with Id " + request.Id));
            }

            foreach(var product in exsistingEstimation.EstimationProducts)
            {
                var requestProduct = request.EstimationProducts.Where(e => e.OrderNumber.Equals(product.OrderDocument.Deserialize<OrderDocumentValueObject>().OrderNumber)).FirstOrDefault();
                var productGrade = new ProductGrade(requestProduct.GradeA, requestProduct.GradeB, requestProduct.GradeC, requestProduct.GradeD);
                product.SetProductGrade(productGrade);
                product.SetTotalGramEstimation(requestProduct.TotalGramEstimation);
            }

            await _estimationProductRepository.Update(exsistingEstimation);
            _storage.Save();

            return exsistingEstimation;
        }
    }
}
