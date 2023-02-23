using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Loom.Queries
{
    public interface IDailyOperationLoomBeamsUsedQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetAllBeamsUsed();
        //Task<IEnumerable<TModel>> GetBeamsUsedById(Guid id);
        //Task<IEnumerable<TModel>> GetBeamsUsedByIdProcessed(Guid id);
    }
}
