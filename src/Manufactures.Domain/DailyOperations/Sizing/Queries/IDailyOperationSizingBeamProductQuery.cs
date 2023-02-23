using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Sizing.Queries
{
    public interface IDailyOperationSizingBeamProductQuery<TModel>
    {
        Task<List<TModel>> GetSizingBeamProductsByOrder(Guid orderId);
    }
}
