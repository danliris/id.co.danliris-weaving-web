using Barebone.Tests;
using Manufactures.Application.Estimations.Productions.DataTransferObjects;
using Manufactures.Controllers.Api;
using Manufactures.Data.EntityFrameworkCore.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.Productions.Queries;
using Manufactures.Domain.Estimations.Productions.ReadModels;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Queries;
using Manufactures.Domain.Estimations.WeavingEstimationProductions.Repositories;
using Manufactures.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Estimations.Controllers
{
    public class EstimationControllerTests : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IWeavingEstimatedProductionRepository> mockWeavingEstimatedProductionRepository;

        private readonly Mock<IWeavingEstimatedProductionQuery<WeavingEstimatedProductionDto>> _mockweavingestimationQuery;
        private readonly Mock<IEstimatedProductionDocumentQuery<EstimatedProductionListDto>> _mockestimationQuery;
      


        public EstimationControllerTests() : base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockWeavingEstimatedProductionRepository = this.mockRepository.Create<IWeavingEstimatedProductionRepository>();

            this._mockweavingestimationQuery = this.mockRepository.Create<IWeavingEstimatedProductionQuery<WeavingEstimatedProductionDto>>();
            this._mockestimationQuery = this.mockRepository.Create<IEstimatedProductionDocumentQuery<EstimatedProductionListDto>>();

            this._MockStorage.Setup(x => x.GetRepository<IWeavingEstimatedProductionRepository>()).Returns(mockWeavingEstimatedProductionRepository.Object);

        }
        public EstimationController CreateEstimationController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            EstimationController controller = new EstimationController(_MockServiceProvider.Object, _mockestimationQuery.Object, _mockweavingestimationQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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
            var unitUnderTest = CreateEstimationController();
            var result = await unitUnderTest.UploadFile(DateTime.Now.Month.ToString(), DateTime.Now.Year, DateTime.Now.Month);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetTroubleMachineall()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            WeavingEstimatedProductionDto dto = new WeavingEstimatedProductionDto();
            
            dto.CreatedDate = _date.ToString();


            List<WeavingEstimatedProductionDto> newList = new List<WeavingEstimatedProductionDto>();
            newList.Add(dto);
            IEnumerable<WeavingEstimatedProductionDto> ienumData = newList;
            this._mockweavingestimationQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateEstimationController();
            // Act
            var result = await unitUnderTest.GetWeavingEstimated();

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }

        [Fact]
        public async Task GetTroubleMachineall_keyWord()
        {

            DateTime _date = new DateTime();
            WeavingEstimatedProductionDto dto = new WeavingEstimatedProductionDto();
            
            dto.YearPeriode = "2023";
            dto.Month = "Month";
            dto.CreatedDate = _date.ToString();


            List<WeavingEstimatedProductionDto> newList = new List<WeavingEstimatedProductionDto>();
            newList.Add(dto);
            IEnumerable<WeavingEstimatedProductionDto> ienumData = newList;
            this._mockweavingestimationQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateEstimationController();
            // Act
            var result = await unitUnderTest.GetWeavingEstimated(1, 23, "asc", "group", "{}");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        [Fact]
        public async Task GetDataByFilter()
        {

            DateTime _date = new DateTime();
            WeavingEstimatedProductionDto dto = new WeavingEstimatedProductionDto();

            dto.YearPeriode = "2023";
            dto.Month = "Month";
            dto.CreatedDate = _date.ToString();


            List<WeavingEstimatedProductionDto> newList = new List<WeavingEstimatedProductionDto>();
            newList.Add(dto);
            IEnumerable<WeavingEstimatedProductionDto> ienumData = newList;
            this._mockweavingestimationQuery.Setup(s => s.GetAll()).ReturnsAsync(ienumData);
            var unitUnderTest = CreateEstimationController();
            // Act
            var result = await unitUnderTest.GetWeavingEstimated("Month","2023");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
        [Fact]
        public async Task GetReport()
        {

            Guid newGuid = new Guid();
              this.mockWeavingEstimatedProductionRepository
           .Setup(s => s.Query)
            .Returns(new List<WeavingEstimatedProductionReadModel>
            {
                    new WeavingEstimatedProduction(newGuid,1,"Feb",2,"2023","2023","spno","plait",0,0,0,"","","","","","","","","",0,"","",0,0,0,0,0,0,0,0).GetReadModel()
            }.AsQueryable());
            List<WeavingEstimatedProductionDto> dto = new List<WeavingEstimatedProductionDto>();
            dto.Add(new WeavingEstimatedProductionDto
            {
                YearPeriode = DateTime.Now.Year.ToString(),
                Month = "Feb"
            });
            this._mockweavingestimationQuery.Setup(s => s.GetDataByFilter("Feb", "2023")).Returns(new List<WeavingEstimatedProductionDto>());
            var unitUnderTest = CreateEstimationController();
            // Act
            var result = await unitUnderTest.GetWeavingEstimatedExcel("Feb", "2023");

            // Assert
            Assert.Null(result);
        }
    }
}
