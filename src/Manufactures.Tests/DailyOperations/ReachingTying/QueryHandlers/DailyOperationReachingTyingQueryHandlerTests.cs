using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Reaching.DataTransferObjects;
using Manufactures.Application.DailyOperations.Reaching.QueryHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Reaching;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.Repositories;
using Manufactures.Domain.DailyOperations.Reaching.ValueObjects;
using Manufactures.Domain.DailyOperations.ReachingTying.ValueObjects;
using Manufactures.Domain.FabricConstructions;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.ReachingTying.QueryHandlers
{
    public class DailyOperationReachingTyingQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationReachingTyingRepository>
            mockDailyOperationReachingTyingRepo;
        private readonly Mock<IMachineRepository>
            mockMachineRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public DailyOperationReachingTyingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockMachineRepo = this.mockRepository.Create<IMachineRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockDailyOperationReachingTyingRepo = this.mockRepository.Create<IDailyOperationReachingTyingRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IMachineRepository>()).Returns(mockMachineRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IFabricConstructionRepository>()).Returns(mockFabricConstructionRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockBeamRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationReachingTyingRepository>()).Returns(mockDailyOperationReachingTyingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationReachingTyingQueryHandler CreateDailyOperationReachingTyingQueryHandler()
        {
            return new DailyOperationReachingTyingQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();

            //Instantiate Object for Machine
            var firstMachine = new MachineDocument(
                new Guid("98B34DC4-16CD-4DF2-8A62-2C59E078D9DB"),
                "32",
                "Timur",
                new MachineTypeId(new Guid("E6655428-E60B-4923-AF39-875B5BA61CFB")),
                new UnitId(11));
            var secondMachine = new MachineDocument(
                new Guid("8B9EAB2D-9F2E-459F-89E0-E512F6244851"),
                "102",
                "Utara",
                new MachineTypeId(new Guid("D8D99957-D4E5-4337-B4A7-AAE99D51A372")),
                new UnitId(11));
            mockMachineRepo.Setup(x => x.Query).Returns(new List<MachineDocumentReadModel>().AsQueryable());
            mockMachineRepo.Setup(x => x.Find(It.IsAny<IQueryable<MachineDocumentReadModel>>())).Returns(
                new List<MachineDocument>() { firstMachine, secondMachine });

            //Instantiate Object for Fabric Construction
            var firstFabricConstruction = new FabricConstructionDocument(
                new Guid("03A861FC-4A97-40CC-B478-70357FDF3065"),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "Melintang",
                "PLCTD100",
                "PLCTD100",
                33,
                44,
                55,
                1000,
                "PolyCotton100");
            var secondFabricConstruction = new FabricConstructionDocument(
                new Guid("37BB78E5-CC70-4FD8-B92D-E3E58BAB575C"),
                "Cotton12 Lurus 22 11 54 CTNCD12 CTNCD12",
                "Lurus",
                "CTNCD12",
                "CTNCD12",
                22,
                11,
                54,
                2400,
                "Cotton12");
            mockFabricConstructionRepo.Setup(x => x.Query).Returns(new List<FabricConstructionReadModel>().AsQueryable());
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<IQueryable<FabricConstructionReadModel>>())).Returns(
                new List<FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

            //Instantiate Object for Beam
            var firstBeam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            var secondBeam = new BeamDocument(
                new Guid("47017EF1-7C7A-4238-920E-FC3DF98BBC92"),
                "TS33",
                "Sizing",
                33);
            mockBeamRepo.Setup(x => x.Query).Returns(new List<BeamReadModel>().AsQueryable());
            mockBeamRepo.Setup(x => x.Find(It.IsAny<IQueryable<BeamReadModel>>())).Returns(
                new List<BeamDocument>() { firstBeam, secondBeam });

            //Instantiate Object for Daily Operation Reaching-Tying
            var firstDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(Guid.NewGuid()),
                new UnitId(11),
                new ConstructionId(Guid.NewGuid()),
                new BeamId(Guid.NewGuid()),
                12,
                new DailyOperationReachingValueObject("Plain", "Lurus", 17),
                new DailyOperationTyingValueObject(164, 90, 127),
                OperationStatus.ONFINISH);
            var firstDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                new ShiftId(Guid.NewGuid()),
                MachineStatus.ONCOMPLETE);
            firstDocument.AddDailyOperationReachingDetail(firstDetail);

            var secondDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(Guid.NewGuid()),
                new UnitId(11),
                new ConstructionId(Guid.NewGuid()),
                new BeamId(Guid.NewGuid()),
                17,
                new DailyOperationReachingValueObject("Plain", "Twist", 12),
                new DailyOperationTyingValueObject(122, 76, 128),
                OperationStatus.ONFINISH);
            var secondDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(Guid.NewGuid()),
                DateTimeOffset.UtcNow,
                new ShiftId(Guid.NewGuid()),
                MachineStatus.ONCOMPLETE);
            secondDocument.AddDailyOperationReachingDetail(secondDetail);
            mockDailyOperationReachingTyingRepo.Setup(x => x.Query).Returns(new List<DailyOperationReachingTyingReadModel>().AsQueryable());
            mockDailyOperationReachingTyingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationReachingTyingReadModel>>())).Returns(
                new List<DailyOperationReachingTyingDocument>() { firstDocument, secondDocument });

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var dailyOperationReachingTyingQueryHandler = this.CreateDailyOperationReachingTyingQueryHandler();
            Guid id = Guid.NewGuid();

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetById(
                id);

            // Assert
            Assert.True(false);
        }
    }
}
