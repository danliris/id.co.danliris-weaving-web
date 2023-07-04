using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.DailyOperations.Productions;
using Manufactures.Domain.DailyOperations.Productions.Commands;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Moonlay;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Shared.ValueObjects;

namespace Manufactures.Application.DailyOperations.Production.CommandHandlers
{
    public class UpdateDailyOperationMachineSizingCommandHandler : ICommandHandler<UpdateDailyOperationMachineSizingCommand, DailyOperationMachineSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationMachineSizingDocumentRepository _dailyOperationMachineSizingDocumentRepository;
        private readonly IDailyOperationMachineSizingDetailRepository _dailyOperationMachineSizingDetailRepository;
        private readonly IFabricConstructionRepository _constructionDocumentRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public UpdateDailyOperationMachineSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationMachineSizingDocumentRepository = _storage.GetRepository<IDailyOperationMachineSizingDocumentRepository>();
            _dailyOperationMachineSizingDetailRepository = _storage.GetRepository<IDailyOperationMachineSizingDetailRepository>();
            _constructionDocumentRepository = _storage.GetRepository<IFabricConstructionRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<DailyOperationMachineSizingDocument> Handle(UpdateDailyOperationMachineSizingCommand request, CancellationToken cancellationToken)
        {
            var estimationDocuments =
                _dailyOperationMachineSizingDocumentRepository
                    .Find(o => o.Identity == request.Id);
            var estimationDocument =
                estimationDocuments
                    .FirstOrDefault();
            var estimationDetails =
                _dailyOperationMachineSizingDetailRepository
                    .Find(o => o.EstimatedProductionDocumentId == estimationDocument.Identity);

            if(estimationDocument == null)
            {
                Validator.ErrorValidation(("EstimationDocumentId", "Estimasi Produksi dengan Id " + request.Id + " Tidak Ditemukan"));
            }

            //Update Data
            //var updatedDetails = request.EstimatedDetails.Where(o => estimationDetails.Any(d => d.Identity == o.Identity));
            foreach (var updatedDetail in request.EstimatedDetails)
            {
                var gradeALimit = Math.Round(updatedDetail.GradeA, 4);
                var gradeBLimit = Math.Round(updatedDetail.GradeB, 4);
                var gradeCLimit = Math.Round(updatedDetail.GradeC, 4);
                var gradeDLimit = Math.Round(updatedDetail.GradeD, 4);

                var dbDetail = estimationDetails.Find(o => o.Identity == updatedDetail.Id);
                
                dbDetail.SetGradeA(gradeALimit);
                dbDetail.SetGradeB(gradeBLimit);
                dbDetail.SetGradeC(gradeCLimit);
                dbDetail.SetGradeD(gradeDLimit);

                dbDetail.SetModified();

                await _dailyOperationMachineSizingDetailRepository.Update(dbDetail);
            }

            estimationDocument.SetModified();

            await _dailyOperationMachineSizingDocumentRepository.Update(estimationDocument);

            _storage.Save();

            return estimationDocument;
        }
    }
}
