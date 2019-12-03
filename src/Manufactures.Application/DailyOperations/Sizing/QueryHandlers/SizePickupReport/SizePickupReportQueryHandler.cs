using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Sizing.DataTransferObjects.SizePickupReport;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Queries.SizePickupReport;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Sizing.QueryHandlers.SizePickupReport
{
    public class SizePickupReportQueryHandler : ISizePickupReportQuery<SizePickupReportListDto>
    {
        //protected readonly IHttpClientService
        //    _http;
        private readonly IStorage
            _storage;
        private readonly IDailyOperationSizingRepository
            _dailyOperationSizingRepository;
        private readonly IWeavingOrderDocumentRepository
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
            _fabricConstructionRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IOperatorRepository
            _operatorRepository;

        public SizePickupReportQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {
            //_http =
            //    serviceProvider.GetService<IHttpClientService>();
            _storage =
                storage;
            _dailyOperationSizingRepository =
                _storage.GetRepository<IDailyOperationSizingRepository>();
            _weavingOrderDocumentRepository =
                _storage.GetRepository<IWeavingOrderDocumentRepository>();
            _fabricConstructionRepository =
                _storage.GetRepository<IFabricConstructionRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
            _operatorRepository =
                _storage.GetRepository<IOperatorRepository>();
        }

        public async Task<(IEnumerable<SizePickupReportListDto>, int)> GetReports(string shiftId,
                                                                                  string spuStatus,
                                                                                  int unitId,
                                                                                  DateTimeOffset? date,
                                                                                  DateTimeOffset? dateFrom,
                                                                                  DateTimeOffset? dateTo,
                                                                                  int? month,
                                                                                  int page,
                                                                                  int size,
                                                                                  string order = "{}")
        {
            try
            {
                //Add Shell (result) for Daily Operation Sizing Report Dto
                var result = new List<SizePickupReportListDto>();

                //Query for Daily Operation Sizing
                var dailyOperationSizingQuery =
                    _dailyOperationSizingRepository
                        .Query
                        .Include(o => o.SizingBeamProducts)
                        .Include(o => o.SizingHistories)
                        .Where(o => o.OperationStatus.Equals(OperationStatus.ONFINISH))
                        .AsQueryable();

                //Get Daily Operation Sizing Data from Daily Operation Sizing Repo
                var dailyOperationSizingDocuments =
                    _dailyOperationSizingRepository
                        .Find(dailyOperationSizingQuery.OrderByDescending(x => x.CreatedDate));

                foreach (var sizingDocument in dailyOperationSizingDocuments)
                {

                    //Get Order Production Number
                    await Task.Yield();
                    var orderDocumentId = sizingDocument.OrderDocumentId.Value;
                    var orderDocumentQuery =
                        _weavingOrderDocumentRepository
                            .Query
                            .OrderByDescending(o => o.CreatedDate);

                    var orderDocuments =
                        _weavingOrderDocumentRepository
                            .Find(orderDocumentQuery)
                            .Where(o => o.Identity.Equals(orderDocumentId));

                    //Instantiate New Value if Unit Id not 0
                    if (unitId != 0)
                    {
                        orderDocuments = orderDocuments.Where(x => x.UnitId.Value == unitId);
                    }

                    //Get First Element from Order Documents
                    var orderDocument = orderDocuments.FirstOrDefault();
                    if (orderDocument == null)
                    {
                        continue;
                    }

                    //Get Histories
                    var sizingHistories = sizingDocument.SizingHistories.OrderByDescending(x => x.CreatedDate);

                    //Get First History, if Histories = null, skip This Document
                    var firstHistory = sizingHistories.LastOrDefault();     //Use This History to Get History at Preparation State
                    if (firstHistory == null)
                    {
                        continue;
                    }

                    if (date != null)
                    {
                        if (!(date.Value.Date == firstHistory.DateTimeMachine.Date))
                        {
                            continue;
                        }
                    }

                    else if (dateFrom != null && dateTo != null)
                    {
                        if (!(dateFrom.Value.Date <= firstHistory.DateTimeMachine.Date && firstHistory.DateTimeMachine.Date <= dateTo.Value.Date))
                        {
                            continue;
                        }
                    }
                    else if (dateFrom != null && dateTo == null)
                    {
                        if (dateFrom.Value.Date > firstHistory.DateTimeMachine.Date)
                        {
                            continue;
                        }
                    }
                    else if (dateFrom == null && dateTo != null)
                    {
                        if (firstHistory.DateTimeMachine.Date > dateTo.Value.Date)
                        {
                            continue;
                        }
                    }

                    else if (month != 0)
                    {
                        if (!(firstHistory.DateTimeMachine.Month == month))
                        {
                            continue;
                        }
                    }
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(SizePickupReportListDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        result = result.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        result = result.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }

                var pagedResult = result.Skip((page - 1) * size).Take(size);

                return (pagedResult, result.Count);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
