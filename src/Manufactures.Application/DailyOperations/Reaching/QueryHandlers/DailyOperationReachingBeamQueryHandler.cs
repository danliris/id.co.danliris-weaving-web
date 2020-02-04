using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shifts.Repositories;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Reaching.QueryHandlers.DailyOperationReachingReport
{
    public class DailyOperationReachingBeamQueryHandler : IDailyOperationReachingBeamQuery<DailyOperationReachingBeamDto>
    {
        private readonly IStorage _storage;
        private readonly IDailyOperationReachingRepository
            _dailyOperationReachingRepository;
        private readonly IBeamRepository
            _beamRepository;
        private readonly IDailyOperationReachingHistoryRepository
            _dailyOperationReachingHistoryRepository;

        public DailyOperationReachingBeamQueryHandler(IStorage storage)
        {
            _storage = storage;
            _dailyOperationReachingRepository =
                _storage.GetRepository<IDailyOperationReachingRepository>();
            _dailyOperationReachingHistoryRepository = _storage.GetRepository<IDailyOperationReachingHistoryRepository>();
            _beamRepository =
                _storage.GetRepository<IBeamRepository>();
        }

        public async Task<(IEnumerable<DailyOperationReachingBeamDto>, int)> GetReachingBeamProductsByOrder(string orderId,
                                                                                                            string keyword,
                                                                                                            string filter,
                                                                                                            int page,
                                                                                                            int size,
                                                                                                            string order)
        {
            try
            {
                //Add Shell (result) for Daily Operation Reaching Report Dto
                var result = new List<DailyOperationReachingBeamDto>();

                //Query for Daily Operation Reaching
                var reachingBeamQuery =
                    _dailyOperationReachingRepository
                        .Query
                        .Where(o=>o.OperationStatus.Equals(OperationStatus.ONFINISH))
                        .AsQueryable();

                //Check if Order Id Null
                await Task.Yield();
                if (!string.IsNullOrEmpty(orderId))
                {
                    //Parse if Not Null
                    if (Guid.TryParse(orderId, out Guid orderGuid))
                    {
                        reachingBeamQuery = reachingBeamQuery.Where(x => x.OrderDocumentId.Value == orderGuid);
                    }
                    else
                    {
                        return (result, result.Count);
                    }
                }

                await Task.Yield();
                var dailyOperationReachingDocuments =
                        _dailyOperationReachingRepository
                            .Find(reachingBeamQuery);

                foreach (var reachingDocument in dailyOperationReachingDocuments)
                {
                    var histories = _dailyOperationReachingHistoryRepository.Find(s => s.DailyOperationReachingDocumentId == reachingDocument.Identity);
                    //Get Beam Number
                    await Task.Yield();
                    var reachingBeamDocument =
                        _beamRepository
                            .Find(o => o.Identity.Equals(reachingDocument.SizingBeamId.Value))
                            .FirstOrDefault();
                    var reachingBeamId = reachingBeamDocument.Identity;
                    var reachingBeamNumber = reachingBeamDocument.Number;

                    //Get Comb Number
                    await Task.Yield();
                    var combNumber = reachingDocument.CombNumber;

                    var reachingBeam = new DailyOperationReachingBeamDto(reachingBeamId, reachingBeamNumber, combNumber);

                    result.Add(reachingBeam);
                }

                if (!string.IsNullOrEmpty(keyword))
                {
                    await Task.Yield();
                    result = 
                        result
                            .Where(x => x.ReachingBeamNumber
                                         .Contains(keyword, StringComparison.CurrentCultureIgnoreCase))
                            .ToList();
                }

                if (!order.Contains("{}"))
                {
                    Dictionary<string, string> orderDictionary =
                        JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                    var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                              orderDictionary.Keys.First().Substring(1);
                    System.Reflection.PropertyInfo prop = typeof(DailyOperationReachingBeamDto).GetProperty(key);

                    if (orderDictionary.Values.Contains("asc"))
                    {
                        await Task.Yield();
                        result = result.OrderBy(x => prop.GetValue(x, null)).ToList();
                    }
                    else
                    {
                        await Task.Yield();
                        result = result.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                    }
                }

                var pagedResult = result.Skip((page - 1) * size).Take(size);

                return (pagedResult, result.Count);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
