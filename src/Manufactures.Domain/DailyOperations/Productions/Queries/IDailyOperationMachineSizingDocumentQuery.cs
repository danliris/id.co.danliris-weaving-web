using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.DailyOperations.Productions.Queries
{
    public interface IDailyOperationMachineSizingDocumentQuery<TModel> : IQueries<TModel>
    {
        Task<TModel> GetByIdUpdate(Guid id);
    }
}
