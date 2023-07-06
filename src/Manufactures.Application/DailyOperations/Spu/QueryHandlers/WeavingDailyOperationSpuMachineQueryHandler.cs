using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;

using Manufactures.Application.Helpers;

using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;

using Manufactures.Domain.DailyOperations.Warping.Repositories;

using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manufactures.Application.DailyOperations.Spu.QueryHandlers
{
    public class WeavingDailyOperationSpuMachineQueryHandler : IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>
    {
        ConverterChecker converter = new ConverterChecker();
        GeneralHelper general = new GeneralHelper();
        private readonly IStorage _storage;
        private readonly IWeavingDailyOperationWarpingMachineRepository _repository;
        public WeavingDailyOperationSpuMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
        {

            _storage =
                storage;

            _repository =
                _storage.GetRepository<IWeavingDailyOperationWarpingMachineRepository>();
        }

       
       

       
        public List<WeavingDailyOperationSpuMachineDto> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string mcNo, string sp, string name, string code)
        {
            var allData = from a in _repository.Query
                          where (mcNo == null || (mcNo != null && mcNo != "" && a.MCNo.Contains(mcNo))) &&
                        (shift == null || (shift != null && shift != "" && a.Shift == shift)) &&
                        (sp == null || (sp != null && sp != "" && a.SP.Contains(sp))) &&
                        (name == null || (name != null && name != "" && a.Name.Contains(name))) &&
                        (code == null || (code != null && code != "" && a.Code.Contains(code))) 
                          select new
                          {
                              code = a.Code,
                              threadNo = a.ThreadNo,
                              shift = a.Shift,
                              sp = a.SP,
                              threadCut = a.ThreadCut,
                              length = a.Length,
                              mcNo = a.MCNo,
                              day = a.Date,
                              name = a.Name,
                              Periode = new DateTime(Convert.ToInt32(a.YearPeriode), a.MonthId, a.Date),
                              a.Length,
                              efficiency=a.Eff
                          };
            var query = (from a in allData
                         where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)
                         select new WeavingDailyOperationSpuMachineDto
                         {
                             MCNo= a.mcNo,
                             Date= a.Periode,
                             Shift=a.shift,
                             Length=a.Length,
                             Efficiency= a.efficiency,
                             ThreadCut=a.threadCut
                         });

            return query.OrderByDescending(a=>a.Date).ToList();
        }

        public Task<IEnumerable<WeavingDailyOperationSpuMachineDto>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<WeavingDailyOperationSpuMachineDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
