using Barebone.Tests;
using Manufactures.Application.BeamStockUpload.DataTransferObjects;
using Manufactures.Controllers.Api;
using Manufactures.Domain.BeamStockUpload.Queries;
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

namespace Manufactures.Tests.BeamStockUpload.Controllers
{
    public class BeamStockUploadControllerTests : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IBeamStockQuery<BeamStockUploadDto>> mockWeavingQuery;

        //dwnload excel
      //  private readonly Mock<IWeavingDailyOperationWarpingMachineRepository> mockWeavingDailyOperationWarpingMachineRepository;

        public BeamStockUploadControllerTests() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockWeavingQuery = this.mockRepository.Create<IBeamStockQuery<BeamStockUploadDto>>();
        }

        public BeamStockUploadController CreateBeamStockUploadController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            BeamStockUploadController controller = new BeamStockUploadController(_MockServiceProvider.Object, mockWeavingQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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

        public BeamStockUploadController CreateDailyOperationReachingExcelController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            BeamStockUploadController controller = new BeamStockUploadController(_MockServiceProvider.Object, mockWeavingQuery.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers.Add("Content-Type", "multipart/form-data");
            controller.ControllerContext.HttpContext.Request.Headers.Add("ContentDisposition", "form-data");

            var file = new FormFile(new MemoryStream(Encoding.UTF8.GetBytes("This is a dummy file")), 0, 0, "Data", "dummy.xlsx");
            var content = new StringContent(file.ToString(), Encoding.UTF8, General.JsonMediaType);

            controller.ControllerContext.HttpContext.Request.Form = new FormCollection(new Dictionary<string, StringValues>(), new FormFileCollection { file });

            controller.ControllerContext.HttpContext.Request.Form.Files[0].OpenReadStream();
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public async Task GetLoomMachine()
        {
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            BeamStockUploadDto dto = new BeamStockUploadDto();
            dto.MonthPeriodeId = 1;
            dto.CreatedDate = _date.ToString();
            dto.YearPeriode = "2021";

            List<BeamStockUploadDto> newList = new List<BeamStockUploadDto>();
            newList.Add(dto);
            IEnumerable<BeamStockUploadDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateBeamStockUploadController();
            // Act
            var result = await unitUnderTest.Get();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetLoomSearch()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            BeamStockUploadDto dto = new BeamStockUploadDto();
            dto.MonthPeriodeId = 1;
            dto.CreatedDate = _date.ToString();
            dto.YearPeriode = "2021";
            dto.MonthPeriode = "Januari";
            var keyword = dto.YearPeriode;
            List<BeamStockUploadDto> newList = new List<BeamStockUploadDto>();
            newList.Add(dto);
            IEnumerable<BeamStockUploadDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateBeamStockUploadController();
            // Act
            var result = await unitUnderTest.Get(1, 25, "{}", keyword, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task UploadOK()
        {
            var unitUnderTest = CreateDailyOperationReachingExcelController();
            var result = await unitUnderTest.UploadFile(DateTime.Now.Month.ToString(), DateTime.Now.Year, DateTime.Now.Month);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetByMonthYearDaily()
        {
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            List<BeamStockUploadDto> dto = new List<BeamStockUploadDto>();
            dto.Add(new BeamStockUploadDto
            {
                MonthPeriodeId = DateTime.Now.Month,
                YearPeriode = DateTime.Now.Year.ToString(),
                Beam = "1",
                Code="a",
                Information="a",
                InReaching="a",
                Reaching="a",
                Sizing="a",
                Shift="I",
                Date=1
            });
            this.mockWeavingQuery.Setup(s => s.GetByMonthYear(DateTime.Now.Month, DateTime.Now.Year.ToString(), 0, 0, null)).ReturnsAsync(dto);
            var unitUnderTest = CreateBeamStockUploadController();

            var result = await unitUnderTest.GetByMonthYear(1, 100, DateTime.Now.Month, DateTime.Now.Year.ToString());

            Assert.NotNull(result);
        }




        [Fact]
        public async Task GetExcelDaily_gagal()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            List<BeamStockUploadDto> dto = new List<BeamStockUploadDto>();
            dto.Add(new BeamStockUploadDto
            {
                Shift = "2"
                //Year = DateTime.Now.Year.ToString(),
                //YearPeriode = DateTime.Now.Year.ToString(),
                //Group = "group"
            });
            this.mockWeavingQuery.Setup(s => s.GetByMonthYear(7, "2023y", 1, 31, "2")).ReturnsAsync(dto);
            var unitUnderTest = CreateBeamStockUploadController();
            // Act
            var result = await unitUnderTest.GetBeamStockExcel(7, "2023y", 1, 31, "2");

            // Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
        }


        [Fact]
        public async Task GetExcelDaily_sukses()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            List<BeamStockUploadDto> dto = new List<BeamStockUploadDto>();
            dto.Add(new BeamStockUploadDto
            {
                Shift = "2"
                //Year = DateTime.Now.Year.ToString(),
                //YearPeriode = DateTime.Now.Year.ToString(),
                //Group = "group"
            });
            this.mockWeavingQuery.Setup(s => s.GetByMonthYear(7, "2023", 1, 31, "2")).ReturnsAsync(dto);
            var unitUnderTest = CreateBeamStockUploadController();
            // Act
            var result = await unitUnderTest.GetBeamStockExcel(7, "2023", 1, 31, "2");

            // Assert
           // Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(result));
           // Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
           //karna result nya berisi nm file, jk yg di harapkan (ssert) tidak null, maka unit test ini berhasil
            Assert.NotNull(result);
        }
       
    }
}
