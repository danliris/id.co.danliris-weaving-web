using ExtCore.Data.Abstractions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class UpdatePauseDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository> mockDailyOperationSizingRepo;
        private readonly Mock<IBeamRepository> mockBeamRepo;
        public UpdatePauseDailyOperationSizingCommandHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);

            this.mockStorage = this.mockRepository.Create<IStorage>();
            this.mockStorage.Setup(x => x.Save());

            this.mockDailyOperationSizingRepo = this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockDailyOperationSizingRepo.Object);

            mockBeamRepo = mockRepository.Create<IBeamRepository>();
            mockStorage.Setup(x => x.GetRepository<IBeamRepository>())
                 .Returns(mockBeamRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private UpdatePauseDailyOperationSizingCommandHandler UpdatePauseDailyOperationSizingCommandHandler()
        {
            return new UpdatePauseDailyOperationSizingCommandHandler(this.mockStorage.Object);
        }
    }
}
