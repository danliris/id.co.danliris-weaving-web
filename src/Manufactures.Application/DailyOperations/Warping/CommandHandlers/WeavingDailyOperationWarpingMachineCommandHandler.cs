//using ExtCore.Data.Abstractions;
//using Infrastructure.Domain.Commands;
//using Manufactures.Domain.DailyOperations.Warping.Commands;
//using Manufactures.Domain.DailyOperations.Warping.Entities;
//using Manufactures.Domain.DailyOperations.Warping.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Manufactures.Application.DailyOperations.Warping.CommandHandlers
//{
//    public class WeavingDailyOperationWarpingMachineCommandHandler// : ICommandHandler<WeavingDailyOperationWarpingMachineCommand, WeavingDailyOperationWarpingMachine>
//    {
//        //private readonly IStorage _storage;

//        public Task<WeavingDailyOperationWarpingMachine> Handle(WeavingDailyOperationWarpingMachineCommand request, CancellationToken cancellationToken)
//        {
//              //var startRow = 4;
//            //var startCol = 1;
//            //WeavingDailyOperationWarpingMachine data = new WeavingDailyOperationWarpingMachine(
//            //                   Guid.NewGuid(), //
//            //                   0, //tgl
//            //                   Convert.ToInt32(month),//month
//            //                   Convert.ToInt32(year), //year
//            //                   null, null, null, null, null, null, null, null, null, 0, null, 0, null, new DateTime(), new DateTime(), 0, 0, 0, 0, 0, 0, 0); ;
//            //List<WeavingDailyOperationWarpingMachine> weavings = new List<WeavingDailyOperationWarpingMachine>();
//            //int rowIndex = 0;

//            //foreach (var sheet in sheets)
//            //{

//            //    if (sheet.Name.Trim() == "Produksi WP")
//            //    {
//            //        var totalRows = sheet.Dimension.Rows;
//            //        var totalColumns = sheet.Dimension.Columns;
//            //        for (rowIndex = startRow; rowIndex <= totalRows; rowIndex++)
//            //        {
//            //            if (sheet.Cells[rowIndex, startCol].Value != null)
//            //            {
//            //                general.ValidatePeriode(sheet.Cells[rowIndex, 2], new DateTime(year, month, 1));
//            //                data = new WeavingDailyOperationWarpingMachine(
//            //                   Guid.NewGuid(), //
//            //                   Convert.ToInt32(sheet.Cells[rowIndex, startCol].Value), //tgl
//            //                   Convert.ToInt32(month),//month
//            //                   Convert.ToInt32(year), //year
//            //                   null, null, null, null, null, null, null, null, null, 0, null, 0, null, new DateTime(), new DateTime(), 0, 0, 0, 0, 0, 0, 0);
                          
                            
//            //                //general.ConvertDateTime(sheet.Cells[rowIndex, startCol + 1]), //Date
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 2]), //Machine
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 3]), //Repair
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 4]), //Result
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 5]), //Team
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 6]), //Implementer
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 7]), //KasubsieVerification
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 8]), //KasieVerification
//            //                //converter.GenerateValueString(sheet.Cells[rowIndex, startCol + 9]), //Production
                           
                          
                            

//            //            }
//            //        }
//            //    }
//            //}

//            //  await _repository.Update(data);
//            //               _storage.Save();

//        }
//    }
//}
