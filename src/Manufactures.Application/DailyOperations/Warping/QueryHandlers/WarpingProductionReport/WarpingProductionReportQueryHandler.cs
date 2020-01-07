using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Moonlay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.WarpingProductionReport
{
    public class WarpingProductionReportQueryHandler : IWarpingProductionReportQuery<WarpingProductionReportListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IOperatorRepository
            _operatorRepository;
        private readonly IShiftRepository
            _shiftRepository;

        public WarpingProductionReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage =
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
            _shiftRepository =
                _storage.GetRepository<IShiftRepository>();
        }

        public async Task<WarpingProductionReportListDto> GetReports(int month, int year)
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Report Dto
                var result = new WarpingProductionReportListDto();
                var resultPerOperator = new List<PerOperatorProductionListDto>();

                if (month == 0)
                {
                    return (result);
                }
                DateTimeOffset dateTimeFilter =
                    new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));

                //Query for Daily Operation Warping
                var dailyOperationWarpingQuery =
                    _dailyOperationWarpingRepository
                        .Query
                        .Include(o => o.WarpingBeamProducts)
                        .Include(o => o.WarpingHistories)
                        .AsQueryable();

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                await Task.Yield();
                var dailyOperationWarpingDocuments =
                    _dailyOperationWarpingRepository
                        .Find(dailyOperationWarpingQuery
                        .Where(o => o.DateTimeOperation.Year == dateTimeFilter.Year && o.DateTimeOperation.Month == dateTimeFilter.Month)
                        .OrderByDescending(x => x.CreatedDate));

                if (dailyOperationWarpingDocuments == null)
                {
                    return (result);
                }

                var daysOfMonth = DateTime.DaysInMonth(year, month);

                await Task.Yield();
                foreach (var warpingDocument in dailyOperationWarpingDocuments)
                {
                    for (int i = 1; i <= daysOfMonth; i++)
                    {
                        double aGroup = 0;
                        double bGroup = 0;
                        double cGroup = 0;
                        double dGroup = 0;
                        double eGroup = 0;
                        double fGroup = 0;
                        double gGroup = 0;
                        double total = 0;

                        foreach (var warpingHistory in warpingDocument.WarpingHistories
                                                                      .Where(x => x.DateTimeMachine.Day == i &&
                                                                             x.DateTimeMachine.Month == month &&
                                                                             x.DateTimeMachine.Year == year))
                        {
                            //Get Operator Group (Latest History)
                            await Task.Yield();
                            var operatorId = warpingHistory.OperatorDocumentId;
                            var operatorQuery =
                                _operatorRepository
                                    .Query
                                    .OrderByDescending(o => o.CreatedDate);

                            await Task.Yield();
                            var operatorDocument =
                                _operatorRepository
                                    .Find(operatorQuery)
                                    .Where(o => o.Identity.Equals(operatorId))
                                    .FirstOrDefault();

                            switch (operatorDocument.Group)
                            {
                                case "A":
                                    aGroup = aGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "B":
                                    bGroup = bGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "C":
                                    cGroup = cGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "D":
                                    dGroup = dGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "E":
                                    eGroup = eGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "F":
                                    fGroup = fGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                                case "G":
                                    gGroup = gGroup + warpingHistory.WarpingBeamLengthPerOperator;
                                    break;
                            }

                            await Task.Yield();
                            total = aGroup + bGroup + cGroup + dGroup + eGroup + fGroup + gGroup;

                        }
                        await Task.Yield();
                        var productionPerOperator = new PerOperatorProductionListDto(i,
                                                                                     aGroup,
                                                                                     bGroup,
                                                                                     cGroup,
                                                                                     dGroup,
                                                                                     eGroup,
                                                                                     fGroup,
                                                                                     gGroup,
                                                                                     total);
                        resultPerOperator.Add(productionPerOperator);
                    }

                    await Task.Yield();
                    double totalAGroup = 0;
                    double totalBGroup = 0;
                    double totalCGroup = 0;
                    double totalDGroup = 0;
                    double totalEGroup = 0;
                    double totalFGroup = 0;
                    double totalGGroup = 0;
                    double totalAll = 0;

                    await Task.Yield();
                    foreach (var perOperatorLength in resultPerOperator)
                    {
                        totalAGroup = totalAGroup + perOperatorLength.AGroup ?? 0;
                        totalBGroup = totalBGroup + perOperatorLength.BGroup ?? 0;
                        totalCGroup = totalCGroup + perOperatorLength.CGroup ?? 0;
                        totalDGroup = totalDGroup + perOperatorLength.DGroup ?? 0;
                        totalEGroup = totalEGroup + perOperatorLength.EGroup ?? 0;
                        totalFGroup = totalFGroup + perOperatorLength.FGroup ?? 0;
                        totalGGroup = totalGGroup + perOperatorLength.GGroup ?? 0;
                        totalAll = totalAll + perOperatorLength.Total;
                    }

                    await Task.Yield();
                    result.AGroupTotal = totalAGroup;
                    result.BGroupTotal = totalBGroup;
                    result.CGroupTotal = totalCGroup;
                    result.DGroupTotal = totalDGroup;
                    result.EGroupTotal = totalEGroup;
                    result.FGroupTotal = totalFGroup;
                    result.GGroupTotal = totalGGroup;
                    result.TotalAll = totalAll;
                    result.PerOperatorList = resultPerOperator;
                }

                return (result);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
