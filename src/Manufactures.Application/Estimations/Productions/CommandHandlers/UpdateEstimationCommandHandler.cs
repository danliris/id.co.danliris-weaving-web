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

            //Generate Period
            var year = request.Period.Year;
            var month = request.Period.Month;
            var day = request.Period.Day;
            var period = new DateTime(year, month, day);

            //Generate New Estimation Number
            var countEstimationNumber = (estimationDocuments.Where(o => o.Period.Year == year).Count() + 1).ToString();
            var newEstimationNumber = countEstimationNumber.PadLeft(4, '0') + "/" + month.ToString().PadLeft(2, '0') + "-" + year.ToString();

            estimationDocument.SetEstimatedNumber(newEstimationNumber);
            estimationDocument.SetPeriod(period);
            estimationDocument.SetUnit(request.UnitId);
            estimationDocument.SetModified();

            //Exist in both UI and Db, Updated
            var updatedDetails = request.EstimationProducts.Where(o => estimationDetails.Any(d => d.Identity == o.Identity));
            foreach (var updatedDetail in updatedDetails)
            {
                var dbDetail = estimationDetails.Find(o => o.Identity == updatedDetail.Identity);

                dbDetail.SetOrderId(updatedDetail.OrderId);
                dbDetail.SetConstructionId(updatedDetail.ConstructionId);
                dbDetail.SetGradeA(updatedDetail.GradeA);
                dbDetail.SetGradeB(updatedDetail.GradeB);
                dbDetail.SetGradeC(updatedDetail.GradeC);
                dbDetail.SetGradeD(updatedDetail.GradeD);
                dbDetail.SetModified();

                await _estimatedProductionDetailRepository.Update(dbDetail);
            }

            //Exist in UI but not in Db, Added
            var addedDetails = request.EstimationProducts.Where(o => !estimationDetails.Any(d => d.Identity == o.Identity));
            addedDetails
                .Select(o => new EstimatedProductionDetail(Guid.NewGuid(),
                                                           o.OrderId,
                                                           o.ConstructionId,
                                                           o.GradeA,
                                                           o.GradeB,
                                                           o.GradeC,
                                                           o.GradeD,
                                                           estimationDocument.Identity))
                .ToList()
                .ForEach(async o => await _estimatedProductionDetailRepository.Update(o));

            //Exist in Db but not from UI, Deleted
            var deletedDetails = estimationDetails.Where(o => !request.EstimationProducts.Any(d => d.Identity == o.Identity));
            foreach (var deletedDetail in deletedDetails)
            {
                var order =
                    _orderDocumentRepository
                        .Find(o => o.Identity == deletedDetail.OrderId.Value)
                        .FirstOrDefault();
                order.SetOrderStatus(Constants.ONORDER);
                await _orderDocumentRepository.Update(order);

                deletedDetail.SetDeleted();
                await _estimatedProductionDetailRepository.Update(deletedDetail);
            }

            estimationDocument.SetModified();

            await _estimatedProductionDocumentRepository.Update(estimationDocument);

            _storage.Save();

            return estimationDocument;
        }
    }
}
