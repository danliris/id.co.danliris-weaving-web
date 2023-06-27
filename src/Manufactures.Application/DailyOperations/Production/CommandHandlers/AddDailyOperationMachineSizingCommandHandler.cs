using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Productions;
using Manufactures.Domain.DailyOperations.Productions.Commands;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Production.CommandHandlers
{
    public class AddDailyOperationMachineSizingCommandHandler : ICommandHandler<AddNewDailyOperationMachineSizingCommand, DailyOperationMachineSizingDocument>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationMachineSizingDocumentRepository _dailyOperationMachineSizingDocumentRepository;
        private readonly IDailyOperationMachineSizingDetailRepository _dailyOperationMachineSizingDetailRepository;
        private readonly IOrderRepository _orderDocumentRepository;

        public AddDailyOperationMachineSizingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationMachineSizingDocumentRepository = _storage.GetRepository<IDailyOperationMachineSizingDocumentRepository>();
            _dailyOperationMachineSizingDetailRepository = _storage.GetRepository<IDailyOperationMachineSizingDetailRepository>();
            _orderDocumentRepository = _storage.GetRepository<IOrderRepository>();
        }

        public async Task<DailyOperationMachineSizingDocument> Handle(AddNewDailyOperationMachineSizingCommand request, CancellationToken cancellationToken)
        {
            //Orders Query
            var estimations =
                _dailyOperationMachineSizingDocumentRepository
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

            var newEstimationDocument = new DailyOperationMachineSizingDocument(Guid.NewGuid(),
                                                                        estimationNumber,
                                                                        period,
                                                                        request.UnitId);

            await _dailyOperationMachineSizingDocumentRepository.Update(newEstimationDocument);

            foreach(var estimationDetail in request.DailyOperationMachineSizingDetails)
            {
                var order =
                    _orderDocumentRepository
                        .Find(o => o.Identity == estimationDetail.OrderId)
                        .FirstOrDefault();

                var gradeALimit = Math.Round(estimationDetail.GradeA, 4);
                var gradeBLimit = Math.Round(estimationDetail.GradeB, 4);
                var gradeCLimit = Math.Round(estimationDetail.GradeC, 4);
                var gradeDLimit = Math.Round(estimationDetail.GradeD, 4);

                var newEstimationDetail = new DailyOperationMachineSizingDetail(Guid.NewGuid(),
                                                                        new OrderId(estimationDetail.OrderId),
                                                                        order.ConstructionDocumentId,
                                                                        gradeALimit,
                                                                        gradeBLimit,
                                                                        gradeCLimit,
                                                                        gradeDLimit,
                                                                        newEstimationDocument.Identity);

                await _dailyOperationMachineSizingDetailRepository.Update(newEstimationDetail);

                order.SetOrderStatus(Constants.ONESTIMATED);

                await _orderDocumentRepository.Update(order);
            }
           
            _storage.Save();

            return newEstimationDocument;
        }
    }
}
