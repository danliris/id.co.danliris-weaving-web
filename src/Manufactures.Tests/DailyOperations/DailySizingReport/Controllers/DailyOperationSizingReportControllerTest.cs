using Barebone.Tests;

using Manufactures.Application.DailyOperations.Spu.DataTransferObjects;
using Manufactures.Controllers.Api;

using Manufactures.Domain.DailyOperations.Spu.Queries.WeavingDailyOperationSpuMachines;

using Microsoft.AspNetCore.Http;

using Microsoft.AspNetCore.Mvc;

using Moq;
using System;
using System.Collections.Generic;

using System.Net;

using System.Security.Claims;

using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Production.Controllers
{
    public class DailyOperationSizingReportControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>> mockWeavingQuery;

        public DailyOperationSizingReportControllerTest() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockWeavingQuery = this.mockRepository.Create<IWeavingDailyOperationSpuMachineQuery<WeavingDailyOperationSpuMachineDto>>();
        }

        public DailyOperationSpuController CreateDailyOperationSpuController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationSpuController controller = new DailyOperationSpuController(_MockServiceProvider.Object, mockWeavingQuery.Object);
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
        public async Task GetDailyOperationSizing()
        {
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            WeavingDailyOperationSpuMachineDto dto = new WeavingDailyOperationSpuMachineDto();
            dto.MachineSizing = "SZ 1";
      

            List<WeavingDailyOperationSpuMachineDto> newList = new List<WeavingDailyOperationSpuMachineDto>();
            newList.Add(dto);
            IEnumerable<WeavingDailyOperationSpuMachineDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateDailyOperationSpuController();
            // Act
            var result = await unitUnderTest.GetSizingDailyOperationReport(DateTime.Now.AddYears(-5), DateTime.Now, "1", "SZ 1", "A", "{}", "{}", "{}",1,25);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }




    }
}
