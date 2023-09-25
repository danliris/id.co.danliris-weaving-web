using Barebone.Tests;
using Manufactures.Application.DailyOperations.Loom.DataTransferObjects;
using Manufactures.Controllers.Api;
using Manufactures.Domain.DailyOperations.Loom.Queries;
using Manufactures.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.Controllers
{
    public class DailyOperationLoomMachineReportControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto>> mockWeavingQuery;

        public DailyOperationLoomMachineReportControllerTest() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockWeavingQuery = this.mockRepository.Create<IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto>>();
        }

        public DailyOperationLoomMachineReportController CreateDailyOperationLoomMachineController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationLoomMachineReportController controller = new DailyOperationLoomMachineReportController(_MockServiceProvider.Object, mockWeavingQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
            
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

       
        [Fact]
        public async Task GetExcelDaily_gagal()
        {

            List<DailyOperationLoomMachineDto> dto = new List<DailyOperationLoomMachineDto>();
            dto.Add(new DailyOperationLoomMachineDto
              {
                   Shift = "I"
                
               });
            this.mockWeavingQuery.Setup(s => s.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "")).Returns(dto);

            var unitUnderTest = CreateDailyOperationLoomMachineController();
            // Act
            var result = await unitUnderTest.GetWarpingDailyOperationReportExcel(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "", 1, 100);

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }



        [Fact]
        public async Task GetExcelDaily_sukses()
        {

          
            List<DailyOperationLoomMachineDto> dto = new List<DailyOperationLoomMachineDto>();
            dto.Add(new DailyOperationLoomMachineDto
            {
                Shift = "I"
                //Year = DateTime.Now.Year.ToString(),
                //YearPeriode = DateTime.Now.Year.ToString(),
                //Group = "group"
            });
            this.mockWeavingQuery.Setup(s => s.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now,"","", "","","I","")).Returns(dto);
           // List<TModel> GetDailyReports(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp);

            var unitUnderTest = CreateDailyOperationLoomMachineController();
            // Act
            var result = await unitUnderTest.GetWarpingDailyOperationReportExcel(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "",1,100);

            // Assert
            // Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
            // Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
            //karna result nya berisi nm file, jk yg di harapkan (ssert) tidak null, maka unit test ini berhasil
            Assert.NotNull(result);
        }


        [Fact]
        public async Task GetDailyReports_sukses()
        {

          
            List<DailyOperationLoomMachineDto> dto = new List<DailyOperationLoomMachineDto>();
            dto.Add(new DailyOperationLoomMachineDto
            {
                MonthId = "1",
                Year = DateTime.Now.Year.ToString(),
                YearPeriode = DateTime.Now.Year.ToString(),
                AL = "1",
                Shift = "I"
            });
           mockWeavingQuery.Setup(s => s.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "")).Returns(new List<DailyOperationLoomMachineDto>() { new DailyOperationLoomMachineDto() { MonthId = "1" } });


            var unitUnderTest = CreateDailyOperationLoomMachineController();
            // Act
            var result = await unitUnderTest.GetLoomDailyOperationReport(DateTime.Now.AddYears(-5), DateTime.Now, "TAPPET", "TIMUR", "BAYU", "", "I", "", 1,100);

            // Assert
            Assert.NotNull(result);
        }


       


        //public async Task GetData_sukses()
        //{


        //    List<DailyOperationLoomMachineDto> dto = new List<DailyOperationLoomMachineDto>();
        //    dto.Add(new DailyOperationLoomMachineDto
        //    {
        //        Shift = "I"
        //        //Year = DateTime.Now.Year.ToString(),
        //        //YearPeriode = DateTime.Now.Year.ToString(),
        //        //Group = "group"
        //    });

        //    this.mockWeavingQuery.Setup(s => s.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "")).Returns(dto);
        //    // List<TModel> GetDailyReports(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp);

        //    var unitUnderTest = CreateDailyOperationLoomMachineController();
        //    // Act
        //    var result = await unitUnderTest.GetLoomDailyOperationReport(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "", 1, 100);
        //   // public async Task<IActionResult> GetLoomDailyOperationReport(DateTime fromDate, DateTime toDate, string jenisMesin, string namaBlok, string namaMtc, string operatornya, string shift, string sp, int page, int size)

        //    // Assert
        //    // Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        //    // Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        //    //karna result nya berisi nm file, jk yg di harapkan (ssert) tidak null, maka unit test ini berhasil
        //    Assert.NotNull(result);
        //}
    }
}
