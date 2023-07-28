using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Warping.QueryHandlers;
using Manufactures.Data.EntityFrameworkCore.DailyOperations.Warping.Repositories;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Warping.QueryHandlers
{
    public class WeavingDailyOperationMachineQueryHandlerTest
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;
        
        private readonly Mock<IWeavingDailyOperationWarpingMachineRepository> mockWeavingDailyOperationWarpingMachineRepository;

        public WeavingDailyOperationMachineQueryHandlerTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>(); 
            this.mockWeavingDailyOperationWarpingMachineRepository = this.mockRepository.Create<IWeavingDailyOperationWarpingMachineRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IWeavingDailyOperationWarpingMachineRepository>()).Returns(mockWeavingDailyOperationWarpingMachineRepository.Object);
  

        }

        private WeavingDailyOperationWarpingMachineQueryHandler CreateWeavingDailyOperationWarpingMachineQueryHandler()
        {
            return new WeavingDailyOperationWarpingMachineQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
           var queryHandler = this.CreateWeavingDailyOperationWarpingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockWeavingDailyOperationWarpingMachineRepository
               .Setup(s => s.Query)
                .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
                {
                    new WeavingDailyOperationWarpingMachine(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4",1).GetReadModel()
                }.AsQueryable());


            var result = await queryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationWarpingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockWeavingDailyOperationWarpingMachineRepository
               .Setup(s => s.Query)
                .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
                {
                    new WeavingDailyOperationWarpingMachine(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4",1).GetReadModel()
                }.AsQueryable());


            var result = await queryHandler.GetById(newGuid);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReports_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationWarpingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockWeavingDailyOperationWarpingMachineRepository
               .Setup(s => s.Query)
                .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
                {
                    new WeavingDailyOperationWarpingMachine(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4",1).GetReadModel()
                }.AsQueryable());


            var result =  queryHandler.GetReports(_date,_date,"I","mcno","sp","d","code");

            // Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationWarpingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockWeavingDailyOperationWarpingMachineRepository
               .Setup(s => s.Query)
                .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
                {
                    new WeavingDailyOperationWarpingMachine(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4",1).GetReadModel()
                }.AsQueryable());


            var result = queryHandler.Delete(_date.Month.ToString(),_date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReportsDaily_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationWarpingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockWeavingDailyOperationWarpingMachineRepository
               .Setup(s => s.Query)
                .Returns(new List<WeavingDailyOperationWarpingMachineReadModel>
                {
                    new WeavingDailyOperationWarpingMachine(newGuid,1,"month",_date.Day,_date.Year.ToString(),_date.Year.ToString(),"I","mcno","name","group","lot","sp",_date.Year.ToString(),
                    "warpType","al","pp","code","beamno",1,"d",1,"mt",_date,_date,1,2,3,4,5,6,"4",1).GetReadModel()
                }.AsQueryable());


            var result = queryHandler.GetDailyReports(_date, _date, "I", "mcno", "sp", "name", "code");

            // Assert
            result.Should().NotBeNull();
        }

    }
}
