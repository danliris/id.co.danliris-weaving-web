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

                var headers = new List<WarpingProductionReportHeaderDto>();
                var processedBodyList = new List<WarpingProductionReportProcessedListDto>();
                
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
                        var dailyProcessedPerOperatorList = new List<DailyProcessedPerOperatorDto>();

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

                            await Task.Yield();
                            var dailyProcessedPerOperator = new DailyProcessedPerOperatorDto(operatorDocument.Group, 
                                                                                             operatorDocument.CoreAccount.Name, 
                                                                                             warpingHistory.WarpingBeamLengthPerOperator);
                            dailyProcessedPerOperatorList.Add(dailyProcessedPerOperator);

                            var header = new WarpingProductionReportHeaderDto(operatorDocument.Group,
                                                                              operatorDocument.CoreAccount.Name);
                            if (!headers.Any(o => o.Group == header.Group && o.Name == header.Name))
                            {
                                headers.Add(header);
                            }
                        }

                        await Task.Yield();
                        var processedBody = new WarpingProductionReportProcessedListDto(i, dailyProcessedPerOperatorList);
                        processedBodyList.Add(processedBody);
                    }
                    result.Headers = headers;
                    result.ProcessedList = processedBodyList;
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
