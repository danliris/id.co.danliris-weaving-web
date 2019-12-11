using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Reaching.Queries
{
    public interface IDailyOperationReachingBeamQuery<TModel>
    {
        Task<(IEnumerable<TModel>, int)> GetReachingBeamProductsByOrder(string orderId,
                                                                        string keyword,
                                                                        string filter,
                                                                        int page,
                                                                        int size,
                                                                        string order);
    }
}
