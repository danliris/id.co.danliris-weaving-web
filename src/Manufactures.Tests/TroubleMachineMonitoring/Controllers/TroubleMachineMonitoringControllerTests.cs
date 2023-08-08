using Barebone.Tests;
using Manufactures.Application.TroubleMachineMonitoring.DTOs;
using Manufactures.Application.TroubleMachineMonitoring.Queries;
using Manufactures.Controllers.Api;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories;
using Manufactures.Domain.TroubleMachineMonitoring.Queries;
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

namespace Manufactures.Tests.TroubleMachineMonitoring.Controllers
{
    public class TroubleMachineMonitoringControllerTests : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<ITroubleMachineMonitoringQuery> _mocktroubleMachineMonitoring;
        private readonly Mock<IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto>> _mocklosesQuery;
      
        public TroubleMachineMonitoringControllerTests() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this._mocktroubleMachineMonitoring = this.mockRepository.Create<ITroubleMachineMonitoringQuery>();
            this._mocklosesQuery = this.mockRepository.Create<IWeavingTroubleMachineTreeLosesQuery<WeavingTroubleMachingTreeLosesDto>>();

        }
        public TroubleMachineMonitoringController CreateTroubleMachineMonitoringController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            TroubleMachineMonitoringController controller = new TroubleMachineMonitoringController(_MockServiceProvider.Object,_mocktroubleMachineMonitoring.Object, _mocklosesQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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
        public async Task UploadOK()
        {
            var unitUnderTest = CreateTroubleMachineMonitoringController();
            var result = await unitUnderTest.UploadFile(DateTime.Now.Month.ToString(), DateTime.Now.Year, DateTime.Now.Month);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetTroubleMachineall()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            WeavingTroubleMachingTreeLosesDto dto = new WeavingTroubleMachingTreeLosesDto();
            dto.Group = "group";
            dto.CreatedDate = _date.ToString();
             

            List<WeavingTroubleMachingTreeLosesDto> newList = new List<WeavingTroubleMachingTreeLosesDto>();
            newList.Add(dto);
            IEnumerable<WeavingTroubleMachingTreeLosesDto> ienumData = newList;
            this._mocklosesQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateTroubleMachineMonitoringController();
            // Act
            var result = await unitUnderTest.GetWarpingMachine();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetTroubleMachineall_keyWord()
        {
 
            DateTime _date = new DateTime();
            WeavingTroubleMachingTreeLosesDto dto = new WeavingTroubleMachingTreeLosesDto();
            dto.Group = "group";
            dto.YearPeriode = "2023";
            dto.Month = "Month";
            dto.CreatedDate = _date.ToString();
             

            List<WeavingTroubleMachingTreeLosesDto> newList = new List<WeavingTroubleMachingTreeLosesDto>();
            newList.Add(dto);
            IEnumerable<WeavingTroubleMachingTreeLosesDto> ienumData = newList;
            this._mocklosesQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateTroubleMachineMonitoringController();
            // Act
            var result = await unitUnderTest.GetWarpingMachine(1, 23, "asc", "group", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetDataByFilter()
        {

            DateTime _date = new DateTime();
            WeavingTroubleMachingTreeLosesDto dto = new WeavingTroubleMachingTreeLosesDto();

            dto.YearPeriode = "2023";
            dto.Month = "Month";
            dto.CreatedDate = _date.ToString();


            List<WeavingTroubleMachingTreeLosesDto> newList = new List<WeavingTroubleMachingTreeLosesDto>();
            newList.Add(dto);
            IEnumerable<WeavingTroubleMachingTreeLosesDto> ienumData = newList;
            this._mocklosesQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateTroubleMachineMonitoringController();
            // Act
            var result = await unitUnderTest.GetDataByFilter(1, 100, dto.Month, dto.YearPeriode);

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

    }
}
