using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.Orders.Queries
{
    public interface IOrderByUnitAndPeriodQuery<TModel>
    {
        Task<IEnumerable<TModel>> GetByUnitAndPeriod(int month, int year, int unitId);
    }
}
