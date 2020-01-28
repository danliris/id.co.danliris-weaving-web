using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Beams.CommandHandlers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.Commands;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Beams.CommandHandlers
{
    public class RemoveBeamCommandHandlerTests : IDisposable
    {
        private MockRepository mockRepository;
        private Mock<IStorage> mockStorage;
        private Mock<IBeamRepository>
            mockBeamRepo;

        public RemoveBeamCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockBeamRepo =
                this.mockRepository.Create<IBeamRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IBeamRepository>())
                .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RemoveBeamCommandHandler CreateRemoveBeamCommandHandler()
        {
            return new RemoveBeamCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var removeBeamCommandHandler = this.CreateRemoveBeamCommandHandler();
            var testId = Guid.NewGuid();

            var beam = new BeamDocument(testId, "T133", "Sizing", 22);


            //Mocking Setup
            //mockBeamRepo
            //    .Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new BeamDocument { beam });
            mockBeamRepo.
                Setup(s => s.Query)
                .Returns(new List<BeamReadModel>() { beam.GetReadModel() }.AsQueryable());
            mockBeamRepo.Setup(o => o.Update(It.IsAny<BeamDocument>())).Returns(Task.CompletedTask);
            this.mockStorage.Setup(x => x.Save());

            RemoveBeamCommand request = new RemoveBeamCommand() {
                
            };
            request.SetId(testId);
            CancellationToken cancellationToken = default(CancellationToken);

            // Act
            var result = await removeBeamCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().Equals(result.Identity);
        }
    }
}
