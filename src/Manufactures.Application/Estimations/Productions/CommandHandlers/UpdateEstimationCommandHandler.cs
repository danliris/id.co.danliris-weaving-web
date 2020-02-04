using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.Estimations.Productions.CommandHandlers
{
    public class UpdateEstimationCommandHandler : ICommandHandler<UpdateEstimationProductCommand, EstimatedProductionDocument>
    {
        private readonly IStorage _storage;
        private readonly IEstimatedProductionDocumentRepository _estimatedProductionDocumentRepository;
        private readonly IEstimatedProductionDetailRepository _estimatedProductionDetailRepository;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public UpdateEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimatedProductionDocumentRepository = _storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _estimatedProductionDetailRepository = _storage.GetRepository<IEstimatedProductionDetailRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IFabricConstructionRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(UpdateEstimationProductCommand request, CancellationToken cancellationToken)
        {
            var estimationDocuments =
                _estimatedProductionDocumentRepository
                    .Find(o => o.Identity == request.Id);
            var estimationDocument =
                estimationDocuments
                    .FirstOrDefault();
            var estimationDetails =
                _estimatedProductionDetailRepository
                    .Find(o => o.EstimatedProductionDocumentId == estimationDocument.Identity);

            if(estimationDocument == null)
            {
                Validator.ErrorValidation(("EstimationDocumentId", "Estimasi Produksi dengan Id " + request.Id + " Tidak Ditemukan"));
            }

            //Update Data
            //var updatedDetails = request.EstimatedDetails.Where(o => estimationDetails.Any(d => d.Identity == o.Identity));
            foreach (var updatedDetail in request.EstimatedDetails)
            {
                var dbDetail = estimationDetails.Find(o => o.Identity == updatedDetail.Id);
                
                dbDetail.SetGradeA(updatedDetail.GradeA);
                dbDetail.SetGradeB(updatedDetail.GradeB);
                dbDetail.SetGradeC(updatedDetail.GradeC);
                dbDetail.SetGradeD(updatedDetail.GradeD);

                dbDetail.SetModified();

                await _estimatedProductionDetailRepository.Update(dbDetail);
            }

            estimationDocument.SetModified();

            await _estimatedProductionDocumentRepository.Update(estimationDocument);

            _storage.Save();

            return estimationDocument;
        }
    }
}
