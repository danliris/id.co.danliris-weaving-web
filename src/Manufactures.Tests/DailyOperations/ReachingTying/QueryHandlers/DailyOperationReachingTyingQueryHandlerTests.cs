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
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts;
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
        private readonly Mock<IOperatorRepository>
            mockOperatorRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;

        public DailyOperationReachingTyingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockMachineRepo = this.mockRepository.Create<IMachineRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockDailyOperationReachingTyingRepo = this.mockRepository.Create<IDailyOperationReachingTyingRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IMachineRepository>()).Returns(mockMachineRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOperatorRepository>()).Returns(mockOperatorRepo.Object);
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
            //mockMachineRepo.Setup(x => x.Query)
            //    .Returns(new List<MachineDocumentReadModel>()
            //    .AsQueryable());
            //mockMachineRepo.Setup(x => x.Find(
            //    It.IsAny<IQueryable<MachineDocumentReadModel>>()))
            //    .Returns(new List<MachineDocument>() { firstMachine, secondMachine });
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { firstMachine, secondMachine });

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
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

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
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { firstBeam, secondBeam });

            //Instantiate Object fo Operator
            var firstOperator = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            var secondOperator = new OperatorDocument(
                new Guid("B94C2C48-7962-4D7D-A7F5-71D90F828350"),
                new CoreAccount("5AEAA60D14F4850039294B68", 0, "Ana Kwee"),
                new UnitId(11),
                "F",
                "AJL",
                "Operator");

            //Instantiate Object fo Shift
            var firstShift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            var secondShift = new ShiftDocument(
                new Guid("BD17BBBC-60BA-42B9-A91D-C85DEF4990D9"),
                "Siang",
                new TimeSpan(14, 01, 00),
                new TimeSpan(22, 00, 00));

            //Instantiate Object for Daily Operation Reaching-Tying
            var firstDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(firstMachine.Identity),
                new UnitId(11),
                new ConstructionId(firstFabricConstruction.Identity),
                new BeamId(firstBeam.Identity),
                12,
                new DailyOperationReachingValueObject("Plain", "Lurus", 17),
                new DailyOperationTyingValueObject(164, 90, 127),
                OperationStatus.ONFINISH);
            var firstDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(firstOperator.Identity),
                DateTimeOffset.UtcNow,
                new ShiftId(firstShift.Identity),
                MachineStatus.ONCOMPLETE);
            firstDocument.AddDailyOperationReachingDetail(firstDetail);

            var secondDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(secondMachine.Identity),
                new UnitId(11),
                new ConstructionId(secondFabricConstruction.Identity),
                new BeamId(secondBeam.Identity),
                17,
                new DailyOperationReachingValueObject("Plain", "Twist", 12),
                new DailyOperationTyingValueObject(122, 76, 128),
                OperationStatus.ONFINISH);
            var secondDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(secondOperator.Identity),
                DateTimeOffset.UtcNow,
                new ShiftId(secondShift.Identity),
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
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { firstMachine, secondMachine });

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
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

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
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { firstBeam, secondBeam });

            //Instantiate Object fo Operator
            var firstOperator = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            var secondOperator = new OperatorDocument(
                new Guid("B94C2C48-7962-4D7D-A7F5-71D90F828350"),
                new CoreAccount("5AEAA60D14F4850039294B68", 0, "Ana Kwee"),
                new UnitId(11),
                "F",
                "AJL",
                "Operator");

            //Instantiate Object fo Shift
            var firstShift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            var secondShift = new ShiftDocument(
                new Guid("BD17BBBC-60BA-42B9-A91D-C85DEF4990D9"),
                "Siang",
                new TimeSpan(14, 01, 00),
                new TimeSpan(22, 00, 00));

            //Instantiate Object for Daily Operation Reaching-Tying
            var firstDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(firstMachine.Identity),
                new UnitId(11),
                new ConstructionId(firstFabricConstruction.Identity),
                new BeamId(firstBeam.Identity),
                12,
                new DailyOperationReachingValueObject("Plain", "Lurus", 17),
                new DailyOperationTyingValueObject(164, 90, 127),
                OperationStatus.ONFINISH);
            var firstDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(firstOperator.Identity),
                DateTimeOffset.UtcNow,
                new ShiftId(firstShift.Identity),
                MachineStatus.ONCOMPLETE);
            firstDocument.AddDailyOperationReachingDetail(firstDetail);

            var secondDocument = new DailyOperationReachingTyingDocument(
                Guid.NewGuid(),
                new MachineId(secondMachine.Identity),
                new UnitId(11),
                new ConstructionId(secondFabricConstruction.Identity),
                new BeamId(secondBeam.Identity),
                17,
                new DailyOperationReachingValueObject("Plain", "Twist", 12),
                new DailyOperationTyingValueObject(122, 76, 128),
                OperationStatus.ONFINISH);
            var secondDetail = new DailyOperationReachingTyingDetail(
                Guid.NewGuid(),
                new OperatorId(secondOperator.Identity),
                DateTimeOffset.UtcNow,
                new ShiftId(secondShift.Identity),
                MachineStatus.ONCOMPLETE);
            secondDocument.AddDailyOperationReachingDetail(secondDetail);
            mockDailyOperationReachingTyingRepo.Setup(x => x.Query).Returns(new List<DailyOperationReachingTyingReadModel>().AsQueryable());
            mockDailyOperationReachingTyingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationReachingTyingReadModel, bool>>>())).Returns(
                new List<DailyOperationReachingTyingDocument>() { firstDocument, secondDocument });

            // Act
            var result = await dailyOperationReachingTyingQueryHandler.GetById(firstDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
