using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using Manufactures.Domain.BrokenCauses.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.FabricConstructions.ValueObjects;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Manufactures.Application.DailyOperations.Warping.QueryHandlers.WarpingBrokenThreadsReport
{
    public class WarpingBrokenReportQueryHandler : IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto>
    {
        private readonly IStorage
            _storage;
        private readonly IDailyOperationWarpingRepository
            _dailyOperationWarpingRepository;
        private readonly IWeavingOrderDocumentRepository
            _orderRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IWarpingBrokenCauseRepository
            _warpingBrokenCauseRepository;
        private readonly IYarnDocumentRepository
            _yarnRepository;

        public WarpingBrokenReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            _storage =
                storage;
            _dailyOperationWarpingRepository =
                _storage.GetRepository<IDailyOperationWarpingRepository>();
            _orderRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _warpingBrokenCauseRepository =
                _storage.GetRepository<IWarpingBrokenCauseRepository>();
            _yarnRepository =
                _storage.GetRepository<IYarnDocumentRepository>();
        }

        public WarpingBrokenThreadsReportListDto GetReports(int month, int year, string weavingId)
        {
            try
            {
                //Add Shell (result) for Daily Operation Warping Broken Report Dto
                var result = new WarpingBrokenThreadsReportListDto();

                if (month == 0)
                {
                    return result;
                }

                //Get Shift (Latest History)
                var orderQuery =
                _orderRepository
                    .Query
                    .AsQueryable();

                if (string.IsNullOrEmpty(weavingId))
                {
                    if (Guid.TryParse(weavingId, out Guid weavingGuid))
                    {
                        orderQuery = orderQuery.Where(o => o.UnitId.Equals(weavingId));
                    }
                    else
                    {
                        return result;
                    }
                }
                DateTimeOffset dateTimeFilter =
                    new DateTimeOffset(year, month, 1, 0, 0, 0, new TimeSpan(+7, 0, 0));

                var monthName = dateTimeFilter.ToString("MMMM", CultureInfo.CreateSpecificCulture("id-ID"));

                var filteredOrderDocuments = _orderRepository.Find(orderQuery);
                var filteredOrderDocumentIds = filteredOrderDocuments.Select(o => o.Identity);

                var filteredConstructionIds = filteredOrderDocuments.Select(o => o.ConstructionId.Value);
                var constructionQuery = _fabricConstructionRepository.Query.AsQueryable();
                var filteredConstructionDocuments = _fabricConstructionRepository.Find(constructionQuery).Where(o => filteredConstructionIds.Contains(o.Identity));

                var warpIdList = filteredConstructionDocuments.SelectMany(o => o.ListOfWarp).Select(o => o.YarnId);

                //Query for Daily Operation Warping
                var dailyOperationWarpingQuery =
                    _dailyOperationWarpingRepository
                        .Query
                        .Include(o => o.WarpingBeamProducts)
                        .Include(o => o.WarpingHistories)
                        .Where(o => filteredOrderDocumentIds.Contains(o.Identity))
                        .AsQueryable();

                var daysOfMonth = DateTime.DaysInMonth(year, month);

                var groupedBeamProducts = _dailyOperationWarpingRepository.Find(dailyOperationWarpingQuery)
                                                                          .SelectMany(item => item.WarpingBeamProducts)
                                                                          .Where(item => item.LatestDateTimeBeamProduct.Month == month &&
                                                                                         item.LatestDateTimeBeamProduct.Year == year)
                                                                          .ToList();
            }
            catch (Exception)
            {

                throw;
            }
            throw new NotImplementedException();
        }
    }
}
