using Barebone.Tests;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Controllers.Api;
using Manufactures.Domain.DailyOperations.Reaching.Queries;
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

namespace Manufactures.Tests.DailyOperations.Reaching.Controllers
{
    public class DailyOperationMachineReachingControllerTests : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IDailyOperationReachingMachineQuery<DailyOperationMachineReachingDto>> mockWeavingQuery;

        public DailyOperationMachineReachingControllerTests() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockWeavingQuery = this.mockRepository.Create<IDailyOperationReachingMachineQuery<DailyOperationMachineReachingDto>>();
        }

        public DailyOperationMachineReachingController CreateDailyOperationMachineReachingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationMachineReachingController controller = new DailyOperationMachineReachingController(_MockServiceProvider.Object, mockWeavingQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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

        public DailyOperationMachineReachingController CreateDailyOperationReachingExcelController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationMachineReachingController controller = new DailyOperationMachineReachingController(_MockServiceProvider.Object, mockWeavingQuery.Object);
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
        public async Task GetWarpingMachineall()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            DailyOperationMachineReachingDto dto = new DailyOperationMachineReachingDto();
            dto.Group = "group";
            dto.CreatedDate = _date.ToString();
            dto.Year = "2021";

            List<DailyOperationMachineReachingDto> newList = new List<DailyOperationMachineReachingDto>();
            newList.Add(dto);
            IEnumerable<DailyOperationMachineReachingDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateDailyOperationMachineReachingController();
            // Act
            var result = await unitUnderTest.Get();

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
        public async Task GetReportDaily()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            List<DailyOperationMachineReachingDto> dto = new List<DailyOperationMachineReachingDto>();
            dto.Add(new DailyOperationMachineReachingDto
            {
                BeamNo = "name",
                Year = DateTime.Now.Year.ToString(),
                YearPeriode = DateTime.Now.Year.ToString(),
                Group = "group"
            });
            this.mockWeavingQuery.Setup(s => s.GetByMonthYear(DateTime.Now.Month, DateTime.Now.Year.ToString())).ReturnsAsync(dto);
            var unitUnderTest = CreateDailyOperationMachineReachingController();
            // Act
            var result = await unitUnderTest.GetByMonthYear(DateTime.Now.Month, DateTime.Now.Year.ToString());

            // Assert
            Assert.NotNull(result);
        }
    }
}
