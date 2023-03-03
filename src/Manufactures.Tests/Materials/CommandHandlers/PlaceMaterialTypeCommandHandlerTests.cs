using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Materials.CommandHandlers;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Materials.Commands;
using Manufactures.Domain.Materials.ReadModels;
using Manufactures.Domain.Materials.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Materials.CommandHandlers
{
    public class PlaceMaterialTypeCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IMaterialTypeRepository> mockMaterialTypeRepo;

        public PlaceMaterialTypeCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockMaterialTypeRepo =
                this.mockRepository.Create<IMaterialTypeRepository>();
            this.mockStorage
                .Setup(x => x.GetRepository<IMaterialTypeRepository>())
                .Returns(mockMaterialTypeRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private PlaceMaterialTypeCommandHandler CreatePlaceMaterialTypeCommandHandler()
        {
            return new PlaceMaterialTypeCommandHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            // Mocking Object
            var placeMaterialTypeCommandHandler = this.CreatePlaceMaterialTypeCommandHandler();

            List<YarnNumberValueObject> yarnNumberValueObjects = new List<YarnNumberValueObject>();
            YarnNumberValueObject yarnNumberValueObject = new YarnNumberValueObject(Guid.NewGuid(), 
                                                                                    "PC", 
                                                                                    "11",
                                                                                    null, 
                                                                                    "0");
            yarnNumberValueObjects.Add(yarnNumberValueObject);

            PlaceMaterialTypeCommand request = new PlaceMaterialTypeCommand
            {
                Code = "PC",
                Name = "PolyCotton",
                RingDocuments = yarnNumberValueObjects,
                Description = "-" 
            };

            this.mockMaterialTypeRepo
                .Setup(x => x.Find(It.IsAny<Expression<Func<MaterialTypeReadModel, bool>>>()))
                .Returns(new List<MaterialTypeDocument>());

            this.mockMaterialTypeRepo
                .Setup(x => x.Update(It.IsAny<MaterialTypeDocument>()))
                .Returns(Task.FromResult(It.IsAny<MaterialTypeDocument>()));

            this.mockStorage.Setup(x => x.Save());

            CancellationToken cancellationToken = CancellationToken.None;

            // Act
            var result = await placeMaterialTypeCommandHandler.Handle(
                request,
                cancellationToken);

            // Assert
            //Check if has identity
            result.Identity.Should().NotBeEmpty();
            //Check result null
            result.Should().NotBeNull();
        }
    }
}
