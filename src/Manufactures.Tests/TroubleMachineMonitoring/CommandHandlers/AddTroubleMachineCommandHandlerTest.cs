using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.TroubleMachineMonitoring.CommandHandlers;
using Manufactures.Domain.TroubleMachineMonitoring;
using Manufactures.Domain.TroubleMachineMonitoring.Commands;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using Manufactures.Domain.TroubleMachineMonitoring.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.TroubleMachineMonitoring.CommandHandlers
{
    public class AddTroubleMachineCommandHandlerTest : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<ITroubleMachineMonitoringRepository> mockTroubleMachineRepo;

        public AddTroubleMachineCommandHandlerTest()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockTroubleMachineRepo = mockRepository.Create<ITroubleMachineMonitoringRepository>();
            mockStorage.Setup(x => x.GetRepository<ITroubleMachineMonitoringRepository>())
                .Returns(mockTroubleMachineRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private AddTroubleMachineMonitoringCommandHandler CreateTroubleMachineMonitoringCommandHandler()
        {
            return new AddTroubleMachineMonitoringCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            //Arrange
            var unitUnderTest = this.CreateTroubleMachineMonitoringCommandHandler();
            AddTroubleMachineMonitoringCommand request = new AddTroubleMachineMonitoringCommand()
            {
                ContinueDate = DateTime.Now,
                Description = "Test Deskripsi",
                MachineNumber = Guid.NewGuid().ToString(),
                OperatorDocument = Guid.NewGuid().ToString(),
                OrderDocument = Guid.NewGuid().ToString(),
                Process = "Test Process",
                StopDate = DateTime.Now,
                Trouble = "Test Trouble",
                WeavingUnitId = "11",
            };

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Should().NotBeNull();
        }

    }
}
