using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.QueryHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.Beams;
using Manufactures.Domain.Beams.ReadModels;
using Manufactures.Domain.Beams.Repositories;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Warping;
using Manufactures.Domain.DailyOperations.Warping.Entities;
using Manufactures.Domain.DailyOperations.Warping.ReadModels;
using Manufactures.Domain.DailyOperations.Warping.Repositories;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Machines;
using Manufactures.Domain.Machines.ReadModels;
using Manufactures.Domain.Machines.Repositories;
using Manufactures.Domain.MachineTypes;
using Manufactures.Domain.MachineTypes.ReadModels;
using Manufactures.Domain.MachineTypes.Repositories;
using Manufactures.Domain.Materials;
using Manufactures.Domain.Operators;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.Operators.Repositories;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ReadModels;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Shifts;
using Manufactures.Domain.Shifts.ReadModels;
using Manufactures.Domain.Shifts.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.QueryHandlers
{
    public class DailyOperationSizingQueryHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockDailyOperationSizingRepo;
        private readonly Mock<IMachineRepository>
            mockMachineRepo;
        private readonly Mock<IMachineTypeRepository>
            mockMachineTypeRepo;
        private readonly Mock<IOrderRepository>
            mockOrderDocumentRepo;
        private readonly Mock<IFabricConstructionRepository>
            mockFabricConstructionRepo;
        private readonly Mock<IShiftRepository>
            mockShiftRepo;
        private readonly Mock<IBeamRepository>
            mockBeamRepo;
        private readonly Mock<IOperatorRepository>
            mockOperatorRepo;
        private readonly Mock<IDailyOperationWarpingRepository>
            mockDailyOperationWarpingRepo;

        public DailyOperationSizingQueryHandlerTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Default);
            this.mockStorage = this.mockRepository.Create<IStorage>();

            this.mockDailyOperationSizingRepo = this.mockRepository.Create<IDailyOperationSizingRepository>();
            this.mockMachineRepo = this.mockRepository.Create<IMachineRepository>();
            this.mockMachineTypeRepo = this.mockRepository.Create<IMachineTypeRepository>();
            this.mockOrderDocumentRepo = this.mockRepository.Create<IOrderRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockShiftRepo = this.mockRepository.Create<IShiftRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockDailyOperationWarpingRepo = this.mockRepository.Create<IDailyOperationWarpingRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>()).Returns(mockDailyOperationSizingRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IMachineRepository>()).Returns(mockMachineRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IMachineTypeRepository>()).Returns(mockMachineTypeRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOrderRepository>()).Returns(mockOrderDocumentRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IFabricConstructionRepository>()).Returns(mockFabricConstructionRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IShiftRepository>()).Returns(mockShiftRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IBeamRepository>()).Returns(mockBeamRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IOperatorRepository>()).Returns(mockOperatorRepo.Object);
            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationWarpingRepository>()).Returns(mockDailyOperationWarpingRepo.Object);
        }

        public void Dispose()
        {
            this.mockRepository.VerifyAll();
        }

        private DailyOperationSizingQueryHandler CreateDailyOperationSizingQueryHandler()
        {
            return new DailyOperationSizingQueryHandler(
                this.mockStorage.Object);
        }

        [Fact]
        public async Task GetAll_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Fabric Construction Object
            var fabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
            var secondFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
                .Returns(new List<Domain.FabricConstructions.FabricConstructionDocument>() { fabricConstruction, secondFabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                new Guid("0121ACFF-A3F6-463B-AC75-51291C920221"),
                "0002/08-2019",
                new ConstructionId(fabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(50, 40, 10),
                new Composition(30, 50, 20),
                "PC",
                "CD",
                4000,
                "PolyCotton",
                new UnitId(11),
                "OPEN-ORDER");
            var secondOrderDocument = new OrderDocument(
                new Guid("E14E40B8-B67D-4293-AC24-AB2210BEB815"),
                "0001/08-2019",
                new ConstructionId(secondFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(30, 50, 20),
                new Composition(50, 40, 10),
                "CD",
                "CM",
                3500,
                "Cotton",
                new UnitId(11),
                "OPEN-ORDER");
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument, secondOrderDocument });

            //Machine Type Object
            var machineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            var secondMachineTypeDocument = new MachineTypeDocument(
                new Guid("B940085D-772B-42AB-9CFC-F24C6E6A61F8"),
                "Sucker Muller",
                3500,
                "Meter");
            //mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
            //    .Returns(new List<MachineTypeDocument>() { firstMachineTypeDocument, secondMachineTypeDocument });

            //Machine Object
            var machineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(machineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            var secondMachineDocument = new MachineDocument(
                new Guid("7CC36BC1-EAF5-4BED-BBA1-5ECA177F30F8"),
                "124",
                "Selatan",
                new MachineTypeId(secondMachineTypeDocument.Identity),
                new UnitId(14),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machineDocument, secondMachineDocument });

            //Operator Object
            var operatorDocument = new OperatorDocument(
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
            //mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
            //    .Returns(new List<OperatorDocument>() { firstOperator, secondOperator });

            //Shift Object
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            var secondShift = new ShiftDocument(
                new Guid("BD17BBBC-60BA-42B9-A91D-C85DEF4990D9"),
                "Siang",
                new TimeSpan(14, 01, 00),
                new TimeSpan(22, 00, 00));
            //mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
            //    .Returns(new List<ShiftDocument>() { firstShift, secondShift });

            //Beam Object
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
            //mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { firstBeam, secondBeam });

            //Sizing Document Object
            List<BeamId> firstBeamWarping = new List<BeamId>();
            var firstBeamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            firstBeamWarping.Add(firstBeamId);

            List<BeamId> secondBeamWarping = new List<BeamId>();
            var secondBeamId = new BeamId(new Guid("24a745be-0177-42af-96a1-c9a2ef2bdc22"));
            secondBeamWarping.Add(secondBeamId);

            var firstSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(machineDocument.Identity),
                                                                       new OrderId(orderDocument.Identity),
                                                                       firstBeamWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var firstSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(shift.Identity),
                                                                     new OperatorId(operatorDocument.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            firstSizingDocument.AddDailyOperationSizingHistory(firstSizingHistory);

            var firstSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(firstBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            firstSizingDocument.AddDailyOperationSizingBeamProduct(firstSizingBeamProduct);

            //Second Sizing Document
            var secondSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(secondMachineDocument.Identity),
                                                                       new OrderId(secondOrderDocument.Identity),
                                                                       firstBeamWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var secondSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(secondShift.Identity),
                                                                     new OperatorId(secondOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            secondSizingDocument.AddDailyOperationSizingHistory(secondSizingHistory);

            var secondSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(secondBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            secondSizingDocument.AddDailyOperationSizingBeamProduct(secondSizingBeamProduct);
            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { firstSizingDocument, secondSizingDocument });

            // Act
            var result = await unitUnderTest.GetAll();

            // Assert
            result.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task GetById_MachineStatusFinish_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Fabric Construction Object
            var firstFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
            var secondFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
                .Returns(new List<Domain.FabricConstructions.FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

            //Order Object
            var firstOrderDocument = new OrderDocument(
                new Guid("0121ACFF-A3F6-463B-AC75-51291C920221"),
                "0002/08-2019",
                new ConstructionId(firstFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(50, 40, 10),
                new Composition(30, 50, 20),
                "PC",
                "CD",
                4000,
                "PolyCotton",
                new UnitId(11),
                "OPEN-ORDER");
            var secondOrderDocument = new OrderDocument(
                new Guid("E14E40B8-B67D-4293-AC24-AB2210BEB815"),
                "0001/08-2019",
                new ConstructionId(secondFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(30, 50, 20),
                new Composition(50, 40, 10),
                "CD",
                "CM",
                3500,
                "Cotton",
                new UnitId(11),
                "OPEN-ORDER");
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { firstOrderDocument, secondOrderDocument });

            //Machine Type Object
            var firstMachineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            var secondMachineTypeDocument = new MachineTypeDocument(
                new Guid("B940085D-772B-42AB-9CFC-F24C6E6A61F8"),
                "Sucker Muller",
                3500,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { firstMachineTypeDocument, secondMachineTypeDocument });

            //Machine Object
            var firstMachineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(firstMachineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            var secondMachineDocument = new MachineDocument(
                new Guid("7CC36BC1-EAF5-4BED-BBA1-5ECA177F30F8"),
                "124",
                "Selatan",
                new MachineTypeId(secondMachineTypeDocument.Identity),
                new UnitId(14),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { firstMachineDocument, secondMachineDocument });

            //Operator Object
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
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { firstOperator, secondOperator });

            //Shift Object
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
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { firstShift, secondShift });

            //Beam Object
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

            //Warping Document Object
            //Material Type Object
            var firstMaterialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            var secondMaterialType = new MaterialTypeDocument(new Guid("3C6A3834-99BE-445E-A322-2B92AB428C21"),
                                                             "CTN",
                                                             "Cotton",
                                                             "-");
            //First Warping Document Object
            var firstWarpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(firstOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var firstHistory = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(firstShift.Identity),
                                                                new OperatorId(firstOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { firstHistory };

            var firstBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(firstBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { firstBeamProduct };

            //Second Warping Document Object
            var secondWarpingDocument = new DailyOperationWarpingDocument(new Guid("2C2CE3D7-CB11-4BD3-A7AB-AE15E72BFD56"),
                                                                         new OrderId(secondOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var secondHistory = new DailyOperationWarpingHistory(new Guid("AC4DD641-4899-4020-A1CE-1D91FE22EBE6"),
                                                                new ShiftId(secondShift.Identity),
                                                                new OperatorId(secondOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { secondHistory };

            var secondBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("24D50631-592A-4D8D-9188-38EBF4046AA3"),
                                                                        new BeamId(secondBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { secondBeamProduct };
            mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
                new List<DailyOperationWarpingDocument>() { firstWarpingDocument, secondWarpingDocument });

            //Sizing Document Object
            List<BeamId> firstBeamsWarping = new List<BeamId>();
            var firstBeamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            firstBeamsWarping.Add(firstBeamId);

            List<BeamId> secondBeamsWarping = new List<BeamId>();
            var secondBeamId = new BeamId(new Guid("24a745be-0177-42af-96a1-c9a2ef2bdc22"));
            secondBeamsWarping.Add(secondBeamId);

            var firstSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(firstMachineDocument.Identity),
                                                                       new OrderId(firstOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var firstSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(firstShift.Identity),
                                                                     new OperatorId(firstOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            firstSizingDocument.AddDailyOperationSizingHistory(firstSizingHistory);

            var firstSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(firstBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            firstSizingDocument.AddDailyOperationSizingBeamProduct(firstSizingBeamProduct);

            //Second Sizing Document
            var secondSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(secondMachineDocument.Identity),
                                                                       new OrderId(secondOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var secondSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(secondShift.Identity),
                                                                     new OperatorId(secondOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            secondSizingDocument.AddDailyOperationSizingHistory(secondSizingHistory);

            var secondSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(secondBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            secondSizingDocument.AddDailyOperationSizingBeamProduct(secondSizingBeamProduct);
            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { firstSizingDocument, secondSizingDocument });

            // Act
            var result = await unitUnderTest.GetById(firstSizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusEntry_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Fabric Construction Object
            var firstFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
            var secondFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
                .Returns(new List<Domain.FabricConstructions.FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

            //Order Object
            var firstOrderDocument = new OrderDocument(
                new Guid("0121ACFF-A3F6-463B-AC75-51291C920221"),
                "0002/08-2019",
                new ConstructionId(firstFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(50, 40, 10),
                new Composition(30, 50, 20),
                "PC",
                "CD",
                4000,
                "PolyCotton",
                new UnitId(11),
                "OPEN-ORDER");
            var secondOrderDocument = new OrderDocument(
                new Guid("E14E40B8-B67D-4293-AC24-AB2210BEB815"),
                "0001/08-2019",
                new ConstructionId(secondFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(30, 50, 20),
                new Composition(50, 40, 10),
                "CD",
                "CM",
                3500,
                "Cotton",
                new UnitId(11),
                "OPEN-ORDER");
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { firstOrderDocument, secondOrderDocument });

            //Machine Type Object
            var firstMachineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            var secondMachineTypeDocument = new MachineTypeDocument(
                new Guid("B940085D-772B-42AB-9CFC-F24C6E6A61F8"),
                "Sucker Muller",
                3500,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { firstMachineTypeDocument, secondMachineTypeDocument });

            //Machine Object
            var firstMachineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(firstMachineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            var secondMachineDocument = new MachineDocument(
                new Guid("7CC36BC1-EAF5-4BED-BBA1-5ECA177F30F8"),
                "124",
                "Selatan",
                new MachineTypeId(secondMachineTypeDocument.Identity),
                new UnitId(14),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { firstMachineDocument, secondMachineDocument });

            //Operator Object
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
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { firstOperator, secondOperator });

            //Shift Object
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
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { firstShift, secondShift });

            //Beam Object
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

            //Warping Document Object
            //Material Type Object
            var firstMaterialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            var secondMaterialType = new MaterialTypeDocument(new Guid("3C6A3834-99BE-445E-A322-2B92AB428C21"),
                                                             "CTN",
                                                             "Cotton",
                                                             "-");
            //First Warping Document Object
            var firstWarpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(firstOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var firstHistory = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(firstShift.Identity),
                                                                new OperatorId(firstOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { firstHistory };

            var firstBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(firstBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { firstBeamProduct };

            //Second Warping Document Object
            var secondWarpingDocument = new DailyOperationWarpingDocument(new Guid("2C2CE3D7-CB11-4BD3-A7AB-AE15E72BFD56"),
                                                                         new OrderId(secondOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var secondHistory = new DailyOperationWarpingHistory(new Guid("AC4DD641-4899-4020-A1CE-1D91FE22EBE6"),
                                                                new ShiftId(secondShift.Identity),
                                                                new OperatorId(secondOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { secondHistory };

            var secondBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("24D50631-592A-4D8D-9188-38EBF4046AA3"),
                                                                        new BeamId(secondBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { secondBeamProduct };
            mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
                new List<DailyOperationWarpingDocument>() { firstWarpingDocument, secondWarpingDocument });

            //Sizing Document Object
            List<BeamId> firstBeamsWarping = new List<BeamId>();
            var firstBeamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            firstBeamsWarping.Add(firstBeamId);

            List<BeamId> secondBeamsWarping = new List<BeamId>();
            var secondBeamId = new BeamId(new Guid("24a745be-0177-42af-96a1-c9a2ef2bdc22"));
            secondBeamsWarping.Add(secondBeamId);

            var firstSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(firstMachineDocument.Identity),
                                                                       new OrderId(firstOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var firstSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(firstShift.Identity),
                                                                     new OperatorId(firstOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONENTRY,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            firstSizingDocument.AddDailyOperationSizingHistory(firstSizingHistory);

            var firstSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(firstBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            firstSizingDocument.AddDailyOperationSizingBeamProduct(firstSizingBeamProduct);

            //Second Sizing Document
            var secondSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(secondMachineDocument.Identity),
                                                                       new OrderId(secondOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var secondSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(secondShift.Identity),
                                                                     new OperatorId(secondOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONENTRY,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            secondSizingDocument.AddDailyOperationSizingHistory(secondSizingHistory);

            var secondSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(secondBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            secondSizingDocument.AddDailyOperationSizingBeamProduct(secondSizingBeamProduct);
            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { firstSizingDocument, secondSizingDocument });

            // Act
            var result = await unitUnderTest.GetById(firstSizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusNotEntryOrFinish_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Fabric Construction Object
            var firstFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
            var secondFabricConstruction = new Domain.FabricConstructions.FabricConstructionDocument(
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
                .Returns(new List<Domain.FabricConstructions.FabricConstructionDocument>() { firstFabricConstruction, secondFabricConstruction });

            //Order Object
            var firstOrderDocument = new OrderDocument(
                new Guid("0121ACFF-A3F6-463B-AC75-51291C920221"),
                "0002/08-2019",
                new ConstructionId(firstFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(50, 40, 10),
                new Composition(30, 50, 20),
                "PC",
                "CD",
                4000,
                "PolyCotton",
                new UnitId(11),
                "OPEN-ORDER");
            var secondOrderDocument = new OrderDocument(
                new Guid("E14E40B8-B67D-4293-AC24-AB2210BEB815"),
                "0001/08-2019",
                new ConstructionId(secondFabricConstruction.Identity),
                DateTimeOffset.UtcNow.AddDays(-1),
                new Period("August", "2019"),
                new Composition(30, 50, 20),
                new Composition(50, 40, 10),
                "CD",
                "CM",
                3500,
                "Cotton",
                new UnitId(11),
                "OPEN-ORDER");
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { firstOrderDocument, secondOrderDocument });

            //Machine Type Object
            var firstMachineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            var secondMachineTypeDocument = new MachineTypeDocument(
                new Guid("B940085D-772B-42AB-9CFC-F24C6E6A61F8"),
                "Sucker Muller",
                3500,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { firstMachineTypeDocument, secondMachineTypeDocument });

            //Machine Object
            var firstMachineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(firstMachineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            var secondMachineDocument = new MachineDocument(
                new Guid("7CC36BC1-EAF5-4BED-BBA1-5ECA177F30F8"),
                "124",
                "Selatan",
                new MachineTypeId(secondMachineTypeDocument.Identity),
                new UnitId(14),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { firstMachineDocument, secondMachineDocument });

            //Operator Object
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
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { firstOperator, secondOperator });

            //Shift Object
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
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { firstShift, secondShift });

            //Beam Object
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

            //Warping Document Object
            //Material Type Object
            var firstMaterialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            var secondMaterialType = new MaterialTypeDocument(new Guid("3C6A3834-99BE-445E-A322-2B92AB428C21"),
                                                             "CTN",
                                                             "Cotton",
                                                             "-");
            //First Warping Document Object
            var firstWarpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(firstOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var firstHistory = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(firstShift.Identity),
                                                                new OperatorId(firstOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { firstHistory };

            var firstBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(firstBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        firstWarpingDocument.Identity);
            firstWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { firstBeamProduct };

            //Second Warping Document Object
            var secondWarpingDocument = new DailyOperationWarpingDocument(new Guid("2C2CE3D7-CB11-4BD3-A7AB-AE15E72BFD56"),
                                                                         new OrderId(secondOrderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var secondHistory = new DailyOperationWarpingHistory(new Guid("AC4DD641-4899-4020-A1CE-1D91FE22EBE6"),
                                                                new ShiftId(secondShift.Identity),
                                                                new OperatorId(secondOperator.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { secondHistory };

            var secondBeamProduct = new DailyOperationWarpingBeamProduct(new Guid("24D50631-592A-4D8D-9188-38EBF4046AA3"),
                                                                        new BeamId(secondBeam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        secondWarpingDocument.Identity);
            secondWarpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { secondBeamProduct };
            mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
                new List<DailyOperationWarpingDocument>() { firstWarpingDocument, secondWarpingDocument });

            //Sizing Document Object
            List<BeamId> firstBeamsWarping = new List<BeamId>();
            var firstBeamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            firstBeamsWarping.Add(firstBeamId);

            List<BeamId> secondBeamsWarping = new List<BeamId>();
            var secondBeamId = new BeamId(new Guid("24a745be-0177-42af-96a1-c9a2ef2bdc22"));
            secondBeamsWarping.Add(secondBeamId);

            var firstSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(firstMachineDocument.Identity),
                                                                       new OrderId(firstOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var firstSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(firstShift.Identity),
                                                                     new OperatorId(firstOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONSTOP,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            firstSizingDocument.AddDailyOperationSizingHistory(firstSizingHistory);

            var firstSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(firstBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            firstSizingDocument.AddDailyOperationSizingBeamProduct(firstSizingBeamProduct);

            //Second Sizing Document
            var secondSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(secondMachineDocument.Identity),
                                                                       new OrderId(secondOrderDocument.Identity),
                                                                       firstBeamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var secondSizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(secondShift.Identity),
                                                                     new OperatorId(secondOperator.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONSTOP,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            secondSizingDocument.AddDailyOperationSizingHistory(secondSizingHistory);

            var secondSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(secondBeam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            secondSizingDocument.AddDailyOperationSizingBeamProduct(secondSizingBeamProduct);
            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { firstSizingDocument, secondSizingDocument });

            // Act
            var result = await unitUnderTest.GetById(firstSizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
