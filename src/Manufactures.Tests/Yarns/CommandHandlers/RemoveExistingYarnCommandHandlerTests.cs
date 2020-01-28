using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.Yarns.CommandHandlers;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Yarns;
using Manufactures.Domain.Yarns.Commands;
using Manufactures.Domain.Yarns.ReadModels;
using Manufactures.Domain.Yarns.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.Yarns.CommandHandlers
{
    public class RemoveExistingYarnCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IYarnDocumentRepository> mockYarnRepo;

        public RemoveExistingYarnCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockYarnRepo =
                this.mockRepository.Create<IYarnDocumentRepository>();

            this.mockStorage
                .Setup(x => x.GetRepository<IYarnDocumentRepository>())
                .Returns(mockYarnRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private RemoveExsistingYarnCommandHandler CreateRemoveExsistingYarnCommandHandler()
        {
            return new RemoveExsistingYarnCommandHandler(this.mockStorage.Object);
        }

        [Fact]
        public async Task Handle_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var testId = Guid.NewGuid();
            var unitUnderTest = this.CreateRemoveExsistingYarnCommandHandler();
            RemoveExsistingYarnCommand request = new RemoveExsistingYarnCommand();
            request.SetId(testId);
            CancellationToken cancellationToken = CancellationToken.None;

            this.mockYarnRepo
               .Setup(x => x.Find(It.IsAny<Expression<Func<YarnDocumentReadModel, bool>>>()))
               .Returns(new List<YarnDocument>()
               {
                   new YarnDocument(testId, 
                                    "0001", 
                                    "Test", 
                                    "weaving test",
                                     new MaterialTypeId(Guid.Parse("a3b59953-b4ea-4d9e-b7f7-8265170d6d2d")), 
                                    new YarnNumberId(Guid.Parse("9fea181a-8b5d-421d-9367-540e730b4650")))
               });

            this.mockYarnRepo
               .Setup(x => x.Update(It.IsAny<YarnDocument>()))
               .Returns(Task.FromResult(It.IsAny<YarnDocument>()));
            
            

            // Act
            var result = await unitUnderTest.Handle(
                request,
                cancellationToken);

            // Assert
            result.Identity.Should().Equals(result.Identity);
        }
    }
}
