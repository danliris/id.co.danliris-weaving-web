using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Queries
{
    public interface IQueries<TModels>
    {
        Task<List<TModels>> Get(int page,
                                int size,
                                string order,
                                string keyword,
                                string filter);
    }
}
