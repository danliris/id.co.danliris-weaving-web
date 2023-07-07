using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;

using Manufactures.Application.Helpers;

using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;

//using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Productions.Repositories;

using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace Manufactures.Application.DailyOperations.Spu.QueryHandlers
//{
//    public class WeavingDailyOperationSpuMachineQueryHandler : IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>
//    {
//        ConverterChecker converter = new ConverterChecker();
//        GeneralHelper general = new GeneralHelper();
//        private readonly IStorage _storage;
        
//        private readonly IDailyOperationMachineSizingDetailRepository _repository;

//        public WeavingDailyOperationSpuMachineQueryHandler(IStorage storage, IServiceProvider serviceProvider)
//        {

//            _storage =
//                storage;

//            _repository =
//                _storage.GetRepository<IDailyOperationMachineSizingDetailRepository>();
//        }

       
       

       
//        //public List<WeavingDailyOperationSpuMachineDto> GetDailyReports(DateTime fromDate, DateTime toDate, string shift, string machineSizing, string groupui)
//        //{
//        //    var allData = from a in _repository.Query
//        //                  where (machineSizing == null || (machineSizing != null && machineSizing != "" && a.MachineSizing.Contains(machineSizing))) &&
//        //                (shift == null || (shift != null && shift != "" && a.Shift == shift)) &&
//        //                (groupui == null || (groupui != null && groupui != "" && a.Group.Contains(groupui))) 
//        //                  select new
//        //                  {
                             
//        //                      shift = a.Shift,
//        //                      groupui = a.Group,
//        //                      machineSizing = a.MachineSizing,
//        //                      periodeid=a.PeriodeId
                          
//        //                  };
//        //    var query = (from a in allData
//        //                // where (a.Periode.Date >= fromDate.Date && a.Periode.Date <= toDate.Date)
//        //                 select new WeavingDailyOperationSpuMachineDto
//        //                 {
//        //                     MachineSizing = a.machineSizing,
//        //                     Group = a.groupui,
//        //                     Shift=a.shift
//        //                 });

//        //    return query.OrderByDescending(a=>a.Date).ToList();
//        //}

//        public Task<IEnumerable<WeavingDailyOperationSpuMachineDto>> GetAll()
//        {
//            throw new NotImplementedException();
//        }

//        public Task<WeavingDailyOperationSpuMachineDto> GetById(Guid id)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
