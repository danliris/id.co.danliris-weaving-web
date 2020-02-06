using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.WarpingProductionReport
{
    public class WarpingProductionReportQueryHandler : IWarpingProductionReportQuery<WarpingProductionReportListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingHistoryRepository
            _dailyOperationWarpingHistoryRepository;
        private readonly IDailyOperationWarpingBeamProductRepository
            _dailyOperationWarpingBeamProductRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public WarpingProductionReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage =
                storage;
            _dailyOperationWarpingHistoryRepository =
                _storage.GetRepository<IDailyOperationWarpingHistoryRepository>();
            _dailyOperationWarpingBeamProductRepository =
                _storage.GetRepository<IDailyOperationWarpingBeamProductRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public WarpingProductionReportListDto GetReports(int month, int year)
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Production Report Dto
                var result = new WarpingProductionReportListDto();

                if (month == 0)
                {
                    return (result);
                }
                DateTimeOffset dateTimeFilter =
                    new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));

                var monthName = dateTimeFilter.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

                //Query for Daily Operation Warping History
                var warpingHistoryQuery =
                    _dailyOperationWarpingHistoryRepository
                        .Query
                        .AsQueryable();

                var daysOfMonth = DateTime.DaysInMonth(year, month);

                var warpingHistories = _dailyOperationWarpingHistoryRepository.Find(warpingHistoryQuery)
                                                                              .Where(item => item.DateTimeMachine.Month == month &&
                                                                                     item.DateTimeMachine.Year == year &&
                                                                                     item.WarpingBeamLengthPerOperator > 0)
                                                                              .ToList();

                var processedList = new List<WarpingProductionReportProcessedListDto>();
                for (var i = 0; i < daysOfMonth; i++)
                {
                    var selectedHistories = warpingHistories.Where(item => item.DateTimeMachine.Day == i + 1).ToList();
                    var dailyProcessedPerOperator = GroupWarpingHistoriesByDateAndOperator(selectedHistories);
                    processedList.Add(new WarpingProductionReportProcessedListDto(i + 1, dailyProcessedPerOperator));
                }

                var headerOperatorsName = processedList
                    .SelectMany(s => s.DailyProcessedPerOperator)
                    .GroupBy(g => new { g.Group, g.Name })
                    .Select(s => new WarpingProductionReportHeaderDto(s.Key.Group, s.Key.Name))
                    .ToList();

                var headerGroups = headerOperatorsName
                    .GroupBy(g => new { g.Group })
                    .Select(s => new WarpingProductionReportGroupDto(s.Key.Group, s.Count()))
                    .ToList();

                return new WarpingProductionReportListDto(monthName, year.ToString(), headerOperatorsName, headerGroups, processedList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        private List<DailyProcessedPerOperatorDto> GroupWarpingHistoriesByDateAndOperator(List<DailyOperationWarpingHistory> histories)
        {
            var result = new List<DailyProcessedPerOperatorDto>();

            var groupedHistories = histories.GroupBy(item => new { item.OperatorDocumentId, item.WarpingBeamLengthPerOperatorUomId }).Select(s => new { s.Key.OperatorDocumentId, Total = s.Sum(sum => sum.WarpingBeamLengthPerOperator), Uom = s.Key.WarpingBeamLengthPerOperatorUomId });

            var operatorIds = groupedHistories.Select(s => s.OperatorDocumentId.Value).ToList();
            var operatorQuery = _operatorRepository.Query.Where(w => operatorIds.Contains(w.Identity));
            var operators = _operatorRepository.Find(operatorQuery);

            foreach (var history in groupedHistories)
            {
                var selectedOperator = operators.FirstOrDefault(f => f.Identity == history.OperatorDocumentId.Value);
                if (history.Uom.Value == 136){
                    var totalLimit = Math.Round(history.Total, 4);
                    result.Add(new DailyProcessedPerOperatorDto(selectedOperator.Group, selectedOperator.CoreAccount.Name, totalLimit));
                } else if(history.Uom.Value == 195)
                {
                    var meterToYardConvertion = history.Total * 1.09361;
                    var conversionValueLimit = Math.Round(meterToYardConvertion, 4);
                    result.Add(new DailyProcessedPerOperatorDto(selectedOperator.Group, selectedOperator.CoreAccount.Name, conversionValueLimit));
                }
            }

            return result;
        }
    }
}
