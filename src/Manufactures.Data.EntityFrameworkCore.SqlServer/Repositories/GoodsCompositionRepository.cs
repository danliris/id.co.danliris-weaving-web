using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Domain.Repositories
{
    public class GoodsCompositionRepository : EntityRepository<GoodsComposition>, IGoodsCompositionRepository
    {
        public List<GoodsComposition> Find(Expression<Func<GoodsComposition, bool>> condition)
        {
            return Query.Where(condition).ToList();
        }
    }
}