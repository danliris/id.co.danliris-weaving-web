using Barebone.Tests;
using ExtCore.Data.Abstractions;
using FluentAssertions;
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
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.Queries;
using Manufactures.Domain.DailyOperations.Warping.Queries.DailyOperationWarpingReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingBrokenThreadsReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WarpingProductionReport;
using Manufactures.Domain.DailyOperations.Warping.Queries.WeavingDailyOperationWarpingMachines;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.Operators.Queries;
using Manufactures.Domain.Shifts.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.Controllers
{
    public class DailyOperationWarpingControllerTest : BaseControllerUnitTest
    {
        private readonly MockRepository mockRepository;

       
        private readonly Mock<IWeavingDailyOperationWarpingMachineRepository> mockWeavingDailyOperationWarpingMachineRepository;
        private readonly Mock<IDailyOperationWarpingRepository>mockdailyOperationWarpingRepository;
        private readonly Mock<IBeamRepository>mockbeamRepository;
        private readonly Mock<IDailyOperationWarpingHistoryRepository> mockdailyOperationWarpingHistoryRepository;
        private readonly Mock<IDailyOperationWarpingBeamProductRepository> mockdailyOperationWarpingBeamProductRepository;
        private readonly Mock<IDailyOperationWarpingBrokenCauseRepository> mockdailyOperationWarpingBrokenCauseRepository;

        private readonly Mock<IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto>> mockDailyOperation;
        private readonly Mock<IOperatorQuery<OperatorListDto>> mockOperatorQuery;
        private readonly Mock<IShiftQuery<ShiftDto>> mockShiftQuery;
        private readonly Mock<IBeamQuery<BeamListDto>> mockBeamQuery;
        private readonly Mock<IWarpingProductionReportQuery<WarpingProductionReportListDto>> mockWarpingProdQuery;
        private readonly Mock<IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto>> mockWarpBrokenQuery;
        private readonly Mock<IWeavingDailyOperationWarpingMachineQuery<WeavingDailyOperationWarpingMachineDto>> mocWeavingQuery;
        private readonly Mock<IDailyOperationWarpingDocumentQuery<DailyOperationWarpingListDto>> mockDailyWarpDocumentQuery;
       
        public DailyOperationWarpingControllerTest() :base()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
             
            this.mockWeavingDailyOperationWarpingMachineRepository = this.mockRepository.Create<IWeavingDailyOperationWarpingMachineRepository>();
            this.mockdailyOperationWarpingRepository = this.mockRepository.Create<IDailyOperationWarpingRepository>();
            this.mockbeamRepository = this.mockRepository.Create<IBeamRepository>();
            this.mockdailyOperationWarpingHistoryRepository = this.mockRepository.Create<IDailyOperationWarpingHistoryRepository>();
            this.mockdailyOperationWarpingBeamProductRepository = this.mockRepository.Create<IDailyOperationWarpingBeamProductRepository>();
            this.mockdailyOperationWarpingBrokenCauseRepository = this.mockRepository.Create<IDailyOperationWarpingBrokenCauseRepository>();


            this.mockDailyOperation=this.mockRepository.Create<IDailyOperationWarpingReportQuery<DailyOperationWarpingReportListDto>>();
            this.mockOperatorQuery = this.mockRepository.Create<IOperatorQuery<OperatorListDto>>();
            this.mockShiftQuery = this.mockRepository.Create<IShiftQuery<ShiftDto>>();
            this.mockBeamQuery = this.mockRepository.Create<IBeamQuery<BeamListDto>>();
            this.mockWarpingProdQuery = this.mockRepository.Create<IWarpingProductionReportQuery<WarpingProductionReportListDto>>();
            this.mockWarpBrokenQuery = this.mockRepository.Create<IWarpingBrokenThreadsReportQuery<WarpingBrokenThreadsReportListDto>>();
            this.mocWeavingQuery = this.mockRepository.Create<IWeavingDailyOperationWarpingMachineQuery<WeavingDailyOperationWarpingMachineDto>>();
            this.mockDailyWarpDocumentQuery = this.mockRepository.Create<IDailyOperationWarpingDocumentQuery<DailyOperationWarpingListDto>>();

            this._MockStorage.Setup(x => x.GetRepository<IWeavingDailyOperationWarpingMachineRepository>()).Returns(mockWeavingDailyOperationWarpingMachineRepository.Object);
            this._MockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBrokenCauseRepository>()).Returns(mockdailyOperationWarpingBrokenCauseRepository.Object);
            this._MockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingRepository>()).Returns(mockdailyOperationWarpingRepository.Object);
            this._MockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockbeamRepository.Object);
            this._MockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingHistoryRepository>()).Returns(mockdailyOperationWarpingHistoryRepository.Object);
            this._MockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingBeamProductRepository>()).Returns(mockdailyOperationWarpingBeamProductRepository.Object);

        }
        public DailyOperationWarpingController CreateDailyOperationWarpingController()
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            DailyOperationWarpingController controller = new DailyOperationWarpingController(_MockServiceProvider.Object,mockDailyWarpDocumentQuery.Object,mockOperatorQuery.Object,mockShiftQuery.Object,mockBeamQuery.Object,mockDailyOperation.Object,mockWarpingProdQuery.Object,mockWarpBrokenQuery.Object,mocWeavingQuery.Object);//(DailyOperationWarpingController)Activator.CreateInstance(typeof(DailyOperationWarpingController), mockServiceProvider.Object);
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
        public async Task GetReport()
        {

            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            this.mockWeavingDailyOperationWarpingMachineRepository
              .Setup(s => s.Query)
               .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
               {
                    new WeavingDailyOperationWarpingMachine(Guid.NewGuid(),1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4").GetReadModel()
               }.AsQueryable());
            this.mocWeavingQuery.Setup(s => s.GetReports(DateTime.Now, DateTime.Now, "", "", "", "", "")).Returns(new List<WeavingDailyOperationWarpingMachineDto>());
            var unitUnderTest = CreateDailyOperationWarpingController();
            // Act
            var result = await unitUnderTest.GetWarpingProductionReport(DateTime.Now, DateTime.Now,"","","","","");

            // Assert
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(result));
        }
    }
}
