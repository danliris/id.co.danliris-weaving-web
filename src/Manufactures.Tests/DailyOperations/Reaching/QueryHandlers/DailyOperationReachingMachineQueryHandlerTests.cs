using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.QueryHandlers;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Reaching.QueryHandlers
{
    public class DailyOperationReachingMachineQueryHandlerTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;

        private readonly Mock<IDailyOperationReachingMachineRepository> mockReachingRepository;

        public DailyOperationReachingMachineQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this.mockReachingRepository = this.mockRepository.Create<IDailyOperationReachingMachineRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingMachineRepository>()).Returns(mockReachingRepository.Object);


        }

        private DailyOperationReachingMachineQueryHandler CreateWeavingDailyOperationReachingMachineQueryHandler()
        {
            return new DailyOperationReachingMachineQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationReachingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationMachineReachingReadModel>
                {
                    new DailyOperationMachineReaching(newGuid,1,"month",_date.Month,_date.Year.ToString(),_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","").GetReadModel()
                }.AsQueryable());

            var result = await queryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationReachingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationMachineReachingReadModel>
                {
                    new DailyOperationMachineReaching(newGuid,1,"month",_date.Month,_date.Year.ToString(),_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","").GetReadModel()
                }.AsQueryable());


            var result = await queryHandler.GetById(newGuid);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetByMonthYear_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationReachingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationMachineReachingReadModel>
                {
                    new DailyOperationMachineReaching(newGuid,1,"month",_date.Month,_date.Year.ToString(),_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","").GetReadModel()
                }.AsQueryable());


            var result = queryHandler.GetByMonthYear(_date.Month, _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }
        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationReachingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationMachineReachingReadModel>
                {
                    new DailyOperationMachineReaching(newGuid,1,"month",_date.Month,_date.Year.ToString(),_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","").GetReadModel()
                }.AsQueryable());


            var result = queryHandler.Delete(_date.Month.ToString(), _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetReportsDaily_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationReachingMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationMachineReachingReadModel>
                {
                    new DailyOperationMachineReaching(newGuid,1,"month",_date.Month,_date.Year.ToString(),_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","").GetReadModel()
                }.AsQueryable());

            var result = queryHandler.GetDailyReports(_date, _date, "I", "mcno");

            // Assert
            result.Should().NotBeNull();
        }
    }
}
