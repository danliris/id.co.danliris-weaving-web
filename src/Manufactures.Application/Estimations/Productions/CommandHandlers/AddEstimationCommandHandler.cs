using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Estimations.Productions;
using Manufactures.Domain.Estimations.Productions.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Estimations.Productions.Repositories;
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
        private readonly IEstimatedProductionDocumentRepository _estimatedProductionDocumentRepository;
        private readonly IEstimatedProductionDetailRepository _estimatedProductionDetailRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public AddEstimationCommandHandler(IStorage storage)
        {
            _storage = storage;
            _estimatedProductionDocumentRepository = _storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _estimatedProductionDetailRepository = _storage.GetRepository<IEstimatedProductionDetailRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<EstimatedProductionDocument> Handle(AddNewEstimationCommand request, CancellationToken cancellationToken)
        {
            //Orders Query
            var estimations =
                _estimatedProductionDocumentRepository
                    .Query
                    .OrderByDescending(o => o.CreatedDate);

            //Generate Period
            var year = request.Year;
            var month = request.Month;
            var day = request.Day;
            var period = new DateTime(year, month, day);

            //Generate Estimation Number
            var countEstimationNumber = (estimations.Where(o => o.Period.Year == year).Count() + 1).ToString();
            var estimationNumber = countEstimationNumber.PadLeft(4, '0') + "/" + month.ToString().PadLeft(2, '0') + "-" + year.ToString();

            var newEstimationDocument = new EstimatedProductionDocument(Guid.NewGuid(),
                                                                        estimationNumber,
                                                                        period,
                                                                        request.UnitId);

            await _estimatedProductionDocumentRepository.Update(newEstimationDocument);

            foreach(var estimationDetail in request.EstimationDetails)
            {
                var order =
                    _orderDocumentRepository
                        .Find(o => o.Identity == estimationDetail.OrderId)
                        .FirstOrDefault();

                var newEstimationDetail = new EstimatedProductionDetail(Guid.NewGuid(),
                                                                        new OrderId(estimationDetail.OrderId),
                                                                        order.ConstructionDocumentId,
                                                                        estimationDetail.GradeA,
                                                                        estimationDetail.GradeB,
                                                                        estimationDetail.GradeC,
                                                                        estimationDetail.GradeD,
                                                                        newEstimationDocument.Identity);

                await _estimatedProductionDetailRepository.Update(newEstimationDetail);

                order.SetOrderStatus(Constants.ONESTIMATED);

                await _orderDocumentRepository.Update(order);
            }
           
            _storage.Save();

            return newEstimationDocument;
        }
    }
}
