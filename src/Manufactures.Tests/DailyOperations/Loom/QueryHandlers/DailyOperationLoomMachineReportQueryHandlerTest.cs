using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Loom.QueryHandlers;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using Manufactures.Domain.DailyOperations.Loom.ReadModels;
using Manufactures.Domain.DailyOperations.Loom.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Loom.QueryHandlers
{
    public class DailyOperationLoomMachineReportQueryHandlerTest
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IServiceProvider> mockServiceProvider;

        private readonly Mock<IDailyOperationLoomMachineRepository> mockReachingRepository;

        public DailyOperationLoomMachineReportQueryHandlerTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockServiceProvider = this.mockRepository.Create<IServiceProvider>();
            this.mockReachingRepository = this.mockRepository.Create<IDailyOperationLoomMachineRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationLoomMachineRepository>()).Returns(mockReachingRepository.Object);


        }

        private DailyOperationLoomMachineQueryHandler CreateWeavingDailyOperationLoomMachineQueryHandler()
        {
            return new DailyOperationLoomMachineQueryHandler(
                this.mockStorage.Object, this.mockServiceProvider.Object);
        }

       
       

        [Fact]
        public async Task GetDailyReports_StateUnderTest_ExpectedBehavior()
        {
            var queryHandler = this.CreateWeavingDailyOperationLoomMachineQueryHandler();
            Guid newGuid = new Guid();
            DateTime _date = new DateTime();
            mockReachingRepository
               .Setup(s => s.Query)
                .Returns(new List<DailyOperationLoomMachineReadModel>
                {
                    new DailyOperationLoomMachine(newGuid,1,"month",_date.Month,_date.Year.ToString(),"I","","name",
                    "mcNo","code","beamNo","","","","","","","","","","","","","","","","","","","","","","","","","","","","","",
                    "","","","","","","","","","","").GetReadModel()
                }.AsQueryable());


            var result = queryHandler.GetDailyReports(DateTime.Now.AddYears(-5), DateTime.Now, "", "", "", "", "I", "");

            // Assert
            result.Should().NotBeNull();
        }
       

        
    }
}
