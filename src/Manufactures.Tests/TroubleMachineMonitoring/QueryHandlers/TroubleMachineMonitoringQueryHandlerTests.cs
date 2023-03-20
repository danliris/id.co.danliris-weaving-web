using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.TroubleMachineMonitoring.Queries;
using Manufactures.Application.TroubleMachineMonitoring.QueryHandlers;
using Manufactures.Domain.TroubleMachineMonitoring.Entities;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.TroubleMachineMonitoring.QueryHandlers
{
    public class WeavingTroubleMachineTreeLosesQueryHandlerTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;

        private readonly Mock<IWeavingTroubleMachineTreeLosesRepository> mock;
        public WeavingTroubleMachineTreeLosesQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this.mock = this.mockRepository.Create<IWeavingTroubleMachineTreeLosesRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IWeavingTroubleMachineTreeLosesRepository>()).Returns(mock.Object);

        }
        private WeavingTroubleMachineTreeLosesQueryHandler CreateWeavingTroubleMachineTreeLosesQueryHandler()
        {
            return new WeavingTroubleMachineTreeLosesQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingTroubleMachineTreeLosesQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mock
               .Setup(s => s.Query)
                .Returns(new List<WeavingTroubleMachineTreeLosesReadModel>
                {
                    new WeavingTroubleMachineTreeLoses(newGuid,1,"month",_date.Day,_date.Year.ToString(),"I","description","warpingMachineNo","group","code",
                    _date,1,_date,_date).GetReadModel()
                }.AsQueryable());


            var result = await queryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }
        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingTroubleMachineTreeLosesQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mock
              .Setup(s => s.Query)
               .Returns(new List<WeavingTroubleMachineTreeLosesReadModel>
               {
                    new WeavingTroubleMachineTreeLoses(newGuid,1,"month",_date.Day,_date.Year.ToString(),"I","description","warpingMachineNo","group","code",
                    _date,1,_date,_date).GetReadModel()
               }.AsQueryable());

            var result = await queryHandler.GetById(newGuid);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task Delete_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingTroubleMachineTreeLosesQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mock
               .Setup(s => s.Query)
                .Returns(new List<WeavingTroubleMachineTreeLosesReadModel>
                {
                    new WeavingTroubleMachineTreeLoses(newGuid,1,"month",_date.Day,_date.Year.ToString(),"I","description","warpingMachineNo","group","code",
                    _date,1,_date,_date).GetReadModel()
                }.AsQueryable());

            var result = queryHandler.Delete(_date.Month.ToString(), _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetDataByFilter_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingTroubleMachineTreeLosesQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mock
              .Setup(s => s.Query)
               .Returns(new List<WeavingTroubleMachineTreeLosesReadModel>
               {
                    new WeavingTroubleMachineTreeLoses(newGuid,1,"month",_date.Day,_date.Year.ToString(),"I","description","warpingMachineNo","group","code",
                    _date,1,_date,_date).GetReadModel()
               }.AsQueryable());


            var result = queryHandler.GetDataByFilter("month", _date.Year.ToString());

            // Assert
            result.Should().NotBeNull();
        }

    }
}
