using Barebone.Tests;
using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.DailyOperationWarpingReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingBrokenThreadsReport;
using Manufactures.Application.DailyOperations.Warping.DataTransferObjects.WarpingProductionReport;
using Manufactures.Application.Operators.DataTransferObjects;
using Manufactures.Application.Shifts.DTOs;
using Manufactures.Controllers.Api;
using Manufactures.DataTransferObjects.Beams;
using Manufactures.Domain.Beams.Queries;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Productions.ReadModels;
using Manufactures.Domain.DailyOperations.Spu.Entities;
using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;
using Manufactures.Domain.DailyOperations.Spu.ReadModels;
using Manufactures.Domain.DailyOperations.Spu.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WeavingDailyOperationWarpingMachines;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shifts.Queries;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Moq;
using System;
using System.Collections.Generic;

using System.Linq;

using System.Net;

using System.Security.Claims;

using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Spu.Controllers
{
    public class DailyOperationSpuDownloadControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        //private readonly Mock<IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>> mockWeavingQuery;



        private readonly Mock<IWeavingDailyOperationMachineSizingRepository> mockWeavingDailyOperationSpuMachineRepository;
        
     
          private readonly Mock<IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>> mocWeavingSpuQuery;

        public DailyOperationSpuDownloadControllerTest() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            
            this.mockWeavingDailyOperationSpuMachineRepository = this.mockRepository.Create<IWeavingDailyOperationMachineSizingRepository>();

            this.mocWeavingSpuQuery = this.mockRepository.Create<IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>>();

            this._MockStorage.Setup(x => x.GetRepository<IWeavingDailyOperationMachineSizingRepository>()).Returns(mockWeavingDailyOperationSpuMachineRepository.Object);
           

        }
       
         public DailyOperationSpuController CreateDailyOperationSpuController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationSpuController controller = new DailyOperationSpuController(_MockServiceProvider.Object, mocWeavingSpuQuery.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");

            return controller;
        }
 
       
        
      //hampir betul tp msh error
        //[Fact]
        //public async Task GetExcel()
        //{

        //    Guid newGuid = new Guid();
        //    DateTime _date = new DateTime();
        //    this.mockWeavingDailyOperationSpuMachineRepository
        //      .Setup(s => s.Query)
             
        //       .Returns(new List<WeavingDailyOperationMachineSizingReadModel>
        //       {
        //            new WeavingDailyOperationMachineSizings(DateTime.Now,DateTime.Now,newGuid,3,"Maret",
        //            "2023",20,"3","SZ 1","1",
        //            "1","1","warpType","wefttype1","wefttype2",
        //            "al","ap1","ap2","thread","cons",
        //            "buyer","1","cons","warpxweft","1",
        //            "1","1","1","1","1",
        //            "1","1","1","1","1",
        //            "1","1","1","1","1",
        //            "1","1","1","1","1",
        //            "1","1","1","1","1",
        //            "1","1","1","1","1",
        //            "1","1").GetReadModel()
        //       }.AsQueryable());

        //    //WeavingDailyOperationSpuMachineDto
        //    //WeavingDailyOperationWarpingMachineDto
        //    List<WeavingDailyOperationSpuMachineDto> dto = new List<WeavingDailyOperationSpuMachineDto>();
        //    dto.Add(new WeavingDailyOperationSpuMachineDto
        //    {
        //        MachineSizing= "SZ 1",
        //        Date = 1,
               
        //        Periode = DateTime.Now,
        //        YearPeriode = DateTime.Now.Year.ToString(),
        //        Group = "group"
        //    });
        //    this.mocWeavingSpuQuery.Setup(s => s.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now,"1", "SZ 1","1", null, null)).Returns(dto);
        //    var unitUnderTest = CreateDailyOperationSpuController();
        //    // Act
        //    var result = await unitUnderTest.GetWarpingDailyOperationReportExcel(DateTime.Now.AddYears(-5), DateTime.Now, "1", "SZ 1", "1", null, null);

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        //}


        ////bisa
        //[Fact]
        //public async Task GetExcel()
        //{

        //    Guid newGuid = new Guid();
        //    DateTime _date = new DateTime();
        //    this.mockWeavingDailyOperationWarpingMachineRepository
        //      .Setup(s => s.Query)
        //       .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
        //       {
        //            new WeavingDailyOperationWarpingMachine(Guid.NewGuid(),1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
        //            "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4").GetReadModel()
        //       }.AsQueryable());
        //    List<WeavingDailyOperationWarpingMachineDto> dto = new List<WeavingDailyOperationWarpingMachineDto>();
        //    dto.Add(new WeavingDailyOperationWarpingMachineDto
        //    {
        //        Name = "name",
        //        Year = DateTime.Now.Year.ToString(),
        //        YearPeriode = DateTime.Now.Year.ToString(),
        //        Group = "group"
        //    });
        //    this.mocWeavingQuery.Setup(s => s.GetReports(DateTime.MinValue, DateTime.Now, "", "", "", "", "")).Returns(dto);
        //    var unitUnderTest = CreateDailyOperationWarpingController();
        //    // Act
        //    var result = await unitUnderTest.GetWarpingProductionExcel(DateTime.MinValue, DateTime.Now, "", "", "", "", "");

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        //}



    }
}
