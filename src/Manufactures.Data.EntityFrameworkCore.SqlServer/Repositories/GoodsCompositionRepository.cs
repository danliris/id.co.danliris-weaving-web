using Infrastructure.Data.EntityFrameworkCore;
using Manufactures.Domain.Orders.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Manufactures.Domain.Orders.Repositories
{
    public class GoodsCompositionRepository : EntityRepository<GoodsComposition>, IGoodsCompositionRepository
    {
        public List<GoodsComposition> Find(Expression<Func<GoodsComposition, bool>> condition)
        {
            return Query.Where(condition).ToList();
        }
    }
}