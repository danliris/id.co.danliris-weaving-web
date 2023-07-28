using Barebone.Tests;
using Manufactures.Application.DailyOperations.Production.DataTransferObjects;

using Manufactures.Controllers.Api;
using Manufactures.Domain.DailyOperations.Productions.Queries;
using Manufactures.Domain.DailyOperations.WeavingDailyOperationMachineSizing.Queries;
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

namespace Manufactures.Tests.DailyOperations.Production.Controllers
{
    public class DailyOperationSizingControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IDailyOperationMachineSizingDocumentQuery<DailyOperationMachineSizingListDto>> estimatedProductionDocumentQuery, mockEstimatedWeavingQuery;

        private readonly Mock<IWeavingDailyOperationMachineSizingQuery<WeavingDailyOperationMachineSizingDto>> weavingEstimatedProductionQuery, mockWeavingQuery;

        public DailyOperationSizingControllerTest() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockEstimatedWeavingQuery = this.mockRepository.Create<IDailyOperationMachineSizingDocumentQuery<DailyOperationMachineSizingListDto>>();
            this.mockWeavingQuery = this.mockRepository.Create<IWeavingDailyOperationMachineSizingQuery<WeavingDailyOperationMachineSizingDto>>();

        }

      
        public DailyOperationSizingNewController CreateDailyOperationSizingNewController()
            
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationSizingNewController controller = new DailyOperationSizingNewController(_MockServiceProvider.Object, mockEstimatedWeavingQuery.Object, mockWeavingQuery.Object);
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

        public DailyOperationSizingNewController CreateDailyOperationWarpingExcelController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationSizingNewController controller = new DailyOperationSizingNewController(_MockServiceProvider.Object, mockEstimatedWeavingQuery.Object, mockWeavingQuery.Object);
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
        public async Task GetDailyOperationSizing1()
        {
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            WeavingDailyOperationMachineSizingDto dto = new WeavingDailyOperationMachineSizingDto();
     
      

            List<WeavingDailyOperationMachineSizingDto> newList = new List<WeavingDailyOperationMachineSizingDto>();
            newList.Add(dto);
            IEnumerable<WeavingDailyOperationMachineSizingDto> ienumData = newList;
            this.mockWeavingQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateDailyOperationSizingNewController();
            // Act
            var result = await unitUnderTest.GetWeavingDailyOperationMachineSizing(1, 25, "{}",  null, "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }


        [Fact]
        public async Task UploadOK()
        {
            var unitUnderTest = CreateDailyOperationWarpingExcelController();
            var result = await unitUnderTest.UploadFile("Januari","2023",1);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

    }
}
