using Infrastructure.Domain.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manufactures.Domain.Orders.Queries
{
    public interface IOrderQuery<TModel> : IQueries<TModel>
    {
    }
}
