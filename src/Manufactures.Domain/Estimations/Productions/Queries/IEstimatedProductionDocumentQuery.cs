using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.Estimations.Productions.Queries
{
    public interface IEstimatedProductionDocumentQuery<TModel> : IQueries<TModel>
    {
        Task<TModel> GetByIdUpdate(Guid id);
    }
}
