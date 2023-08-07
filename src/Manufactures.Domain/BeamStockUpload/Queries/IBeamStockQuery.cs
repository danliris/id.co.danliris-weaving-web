using Infrastructure.Domain.Queries;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Domain.BeamStockUpload.Queries
{
    public interface IBeamStockQuery<TModel> : IQueries<TModel>
    {
        Task<bool> Upload(ExcelWorksheets sheet, string month, int year, int monthId);
        Task<List<TModel>> GetByMonthYear(int monthId, string year, int datestart, int datefinish, string shift);
    }
}
