using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Loom.Queries
{
    public interface IDailyOperationLoomBeamProductQuery<TModel>
    {
        Task<List<TModel>> GetLoomBeamProductsByOrder(Guid orderId);
    }
}
