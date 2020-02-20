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
    public class RemoveExistingTroubleMachineMonitoringCommandHandlerTest : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<ITroubleMachineMonitoringRepository> mockTroubleMachineRepo;

        public RemoveExistingTroubleMachineMonitoringCommandHandlerTest()
        {
           
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockTroubleMachineRepo =
                this.mockRepository.Create<ITroubleMachineMonitoringRepository>();

            this.mockStorage
                .Setup(x => x.GetRepository<ITroubleMachineMonitoringRepository>())
                .Returns(mockTroubleMachineRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DeleteTroubleMachineCommandHandler RemoveExsistingYarnCommandHandler()
        {
            return new DeleteTroubleMachineCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var unitUnderTest = this.RemoveExsistingYarnCommandHandler();
            RemoveExistingTroubleMachineMonitoringCommand request = new RemoveExistingTroubleMachineMonitoringCommand();
            request.SetId(testId);
            CancellationToken cancellationToken = CancellationToken.None;

            this.mockTroubleMachineRepo
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

            this.mockTroubleMachineRepo
               .Setup(x => x.Update(It.IsAny<TroubleMachineMonitoringDocument>()))
               .Returns(Task.FromResult(It.IsAny<TroubleMachineMonitoringDocument>()));



            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().Equals(result.Identity);
        }
    }
}
