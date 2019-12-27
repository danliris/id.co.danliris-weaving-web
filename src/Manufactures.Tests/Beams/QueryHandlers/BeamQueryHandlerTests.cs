using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Beams.QueryHandlers;
using Manufactures.DataTransferObjects.Beams;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Beams.QueryHandlers
{
    public class BeamQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IBeamRepository> mockBeamRepo;

        public BeamQueryHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();

            mockBeamRepo = mockRepository.Create<IBeamRepository>();
            mockStorage.Setup(x => x.GetRepository<IBeamRepository>())
                .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private BeamQueryHandler CreateBeamQueryHandler()
        {
            return new BeamQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var beamQueryHandler = this.CreateBeamQueryHandler();

            //Instantiate Existing Object
            //Add Beam Document Object
            var beamDocument = new BeamDocument(Guid.NewGuid(),
                                                "S123",
                                                "Sizing",
                                                123);
            var beamDto = new BeamListDto(beamDocument);

            mockBeamRepo.Setup(x => x.Query).Returns(new List<BeamReadModel>()
                .AsQueryable());
            mockBeamRepo.Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>()))
                .Returns(new List<BeamDocument>() { beamDocument });

            // Act
            var result = await beamQueryHandler.GetById(beamDto.Id);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
