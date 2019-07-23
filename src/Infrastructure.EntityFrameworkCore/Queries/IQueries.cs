using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Domain.Queries
{
    public interface IQueries<TModels>
    {
        Task<IEnumerable<TModels>> GetAll();
        Task<TModels> GetById(Guid id);
    }
}
