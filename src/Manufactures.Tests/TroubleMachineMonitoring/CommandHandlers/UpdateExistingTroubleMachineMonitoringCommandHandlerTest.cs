using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.TroubleMachineMonitoring.CommandHandlers;
using Manufactures.Domain.Shared.ValueObjects;
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
    public class UpdateExistingTroubleMachineMonitoringCommandHandlerTest : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<ITroubleMachineMonitoringRepository> mockTroubleMachineMonitoringRepo;

        public UpdateExistingTroubleMachineMonitoringCommandHandlerTest()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockTroubleMachineMonitoringRepo =
                this.mockRepository.Create<ITroubleMachineMonitoringRepository>();

            this.mockStorage
                .Setup(x => x.GetRepository<ITroubleMachineMonitoringRepository>())
                .Returns(mockTroubleMachineMonitoringRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private UpdateTroubleMachineMonitoringCommandHandler CreateUpdateExistingYarnCommandHandler()
        {
            return new UpdateTroubleMachineMonitoringCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var unitUnderTest = this.CreateUpdateExistingYarnCommandHandler();
            UpdateExistingTroubleMachineMonitoringCommand request = new UpdateExistingTroubleMachineMonitoringCommand()
            {
                ContinueDate = DateTime.Now,
                Description = "Deskripsi",
                MachineNumber = Guid.NewGuid().ToString(),
                OperatorDocument = Guid.NewGuid().ToString(),
                OrderDocument = Guid.NewGuid().ToString(),
                Process = "Looming",
                StopDate = DateTime.Now,
                Trouble = "Trouble"                
            };

            request.SetId(testId);

            CancellationToken cancellationToken = CancellationToken.None;

            this.mockTroubleMachineMonitoringRepo
              .Setup(x => x.Find(It.IsAny<Expression<Func<TroubleMachineMonitoringReadModel, bool>>>()))
              .Returns(new List<TroubleMachineMonitoringDocument>()
              {
                   new TroubleMachineMonitoringDocument(testId,
                                    DateTime.Now,
                                    DateTime.Now,
                                    new OrderId(Guid.Parse(Guid.NewGuid().ToString())),
                                    "Tying",
                                    new MachineId(Guid.Parse(Guid.NewGuid().ToString())),
                                    new OperatorId(Guid.Parse(Guid.NewGuid().ToString())),
                                    "Trouble",
                                    "Test")
              });

            this.mockTroubleMachineMonitoringRepo
               .Setup(x => x.Update(It.IsAny<TroubleMachineMonitoringDocument>()))
               .Returns(Task.FromResult(It.IsAny<TroubleMachineMonitoringDocument>()));

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Process.Should().Equals("Looming");
            result.Description.Should().Equals("Deskripsi");
        }
    }
}
