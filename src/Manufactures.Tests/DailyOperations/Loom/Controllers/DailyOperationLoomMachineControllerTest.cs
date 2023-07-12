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
    public class DailyOperationLoomMachineControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto>> mockWeavingQuery;

        public DailyOperationLoomMachineControllerTest() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockWeavingQuery = this.mockRepository.Create<IDailyOperationLoomMachineQuery<DailyOperationLoomMachineDto>>();
        }

        public DailyOperationLoomMachineController CreateDailyOperationLoomMachineController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationLoomMachineController controller = new DailyOperationLoomMachineController(_MockServiceProvider.Object, mockWeavingQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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

        public DailyOperationLoomMachineController CreateDailyOperationReachingExcelController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationLoomMachineController controller = new DailyOperationLoomMachineController(_MockServiceProvider.Object, mockWeavingQuery.Object);
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
            DailyOperationLoomMachineDto dto = new DailyOperationLoomMachineDto();
            dto.MonthPeriodeId = 1;
            dto.CreatedDate = _date.ToString();
            dto.YearPeriode = "2021";

            List<DailyOperationLoomMachineDto> newList = new List<DailyOperationLoomMachineDto>();
            newList.Add(dto);
            IEnumerable<DailyOperationLoomMachineDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateDailyOperationLoomMachineController();
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
            DailyOperationLoomMachineDto dto = new DailyOperationLoomMachineDto();
            dto.MonthPeriodeId = 1;
            dto.CreatedDate = _date.ToString();
            dto.YearPeriode = "2021";
            dto.MonthPeriode = "Januari";
            var keyword = dto.YearPeriode;
            List<DailyOperationLoomMachineDto> newList = new List<DailyOperationLoomMachineDto>();
            newList.Add(dto);
            IEnumerable<DailyOperationLoomMachineDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateDailyOperationLoomMachineController();
            // Act
            var result = await unitUnderTest.Get(1,25,"{}",keyword, "{}");

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
            List<DailyOperationLoomMachineDto> dto = new List<DailyOperationLoomMachineDto>();
            dto.Add(new DailyOperationLoomMachineDto
            {
                MonthId = "1",
                Year = DateTime.Now.Year.ToString(),
                YearPeriode = DateTime.Now.Year.ToString(),
                AL = "1"
            });
            this.mockWeavingQuery.Setup(s => s.GetByMonthYear(DateTime.Now.Month, DateTime.Now.Year.ToString())).ReturnsAsync(dto);
            var unitUnderTest = CreateDailyOperationLoomMachineController();
            // Act
            var result = await unitUnderTest.GetByMonthYear(DateTime.Now.Month, DateTime.Now.Year.ToString());

            // Assert
            Assert.NotNull(result);
        }
    }
}
