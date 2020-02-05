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
using Manufactures.Domain.FabricConstructions;
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
using Manufactures.Domain.Suppliers;
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
        private readonly Mock<IDailyOperationSizingDocumentRepository>
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

            this.mockDailyOperationSizingRepo = this.mockRepository.Create<IDailyOperationSizingDocumentRepository>();
            this.mockMachineRepo = this.mockRepository.Create<IMachineRepository>();
            this.mockMachineTypeRepo = this.mockRepository.Create<IMachineTypeRepository>();
            this.mockOrderDocumentRepo = this.mockRepository.Create<IOrderRepository>();
            this.mockFabricConstructionRepo = this.mockRepository.Create<IFabricConstructionRepository>();
            this.mockShiftRepo = this.mockRepository.Create<IShiftRepository>();
            this.mockBeamRepo = this.mockRepository.Create<IBeamRepository>();
            this.mockOperatorRepo = this.mockRepository.Create<IOperatorRepository>();
            this.mockDailyOperationWarpingRepo = this.mockRepository.Create<IDailyOperationWarpingRepository>();

            this.mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingDocumentRepository>()).Returns(mockDailyOperationSizingRepo.Object);
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
            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Machine Type Object
            var machineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            //mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
            //    .Returns(new List<MachineTypeDocument>() { machineTypeDocument });

            //Machine Object
            var machineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(machineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machineDocument });

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
            //mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
            //    .Returns(new List<ShiftDocument>() { firstShift, secondShift });

            //Beam Object
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            //mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
            //    .Returns(new List<BeamDocument>() { firstBeam, secondBeam });

            //Sizing Document Object
            List<BeamId> beamWarping = new List<BeamId>();
            var beamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            beamWarping.Add(beamId);

            var sizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(machineDocument.Identity),
                                                                       new OrderId(orderDocument.Identity),
                                                                       beamWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var sizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(shift.Identity),
                                                                     new OperatorId(operatorDocument.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            sizingDocument.AddDailyOperationSizingHistory(sizingHistory);

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(beam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            sizingDocument.AddDailyOperationSizingBeamProduct(sizingBeamProduct);

            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingDocumentReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { sizingDocument });

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
            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Machine Type Object
            var machineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { machineTypeDocument });

            //Machine Object
            var machineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(machineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machineDocument });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Shift Object
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Beam Object
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Warping Document Object
            //Material Type Object
            var materialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            //First Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(orderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                new OperatorId(operatorDocument.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(beam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        warpingDocument.Identity);
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            //mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            //mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
            //    new List<DailyOperationWarpingDocument>() { warpingDocument });
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>())).Returns(
                new List<DailyOperationWarpingDocument>() { warpingDocument });

            //Sizing Document Object
            List<BeamId> beamsWarping = new List<BeamId>();
            var beamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            beamsWarping.Add(beamId);

            var sizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(machineDocument.Identity),
                                                                       new OrderId(orderDocument.Identity),
                                                                       beamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var sizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(shift.Identity),
                                                                     new OperatorId(operatorDocument.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONFINISH,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            sizingDocument.AddDailyOperationSizingHistory(sizingHistory);

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(beam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            sizingDocument.AddDailyOperationSizingBeamProduct(sizingBeamProduct);

            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingDocumentReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { sizingDocument });

            // Act
            var result = await unitUnderTest.GetById(sizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusEntry_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Machine Type Object
            var machineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { machineTypeDocument });

            //Machine Object
            var machineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(machineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machineDocument });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Shift Object
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Beam Object
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Warping Document Object
            //Material Type Object
            var materialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            //First Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(orderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                new OperatorId(operatorDocument.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(beam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        warpingDocument.Identity);
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            //mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            //mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
            //    new List<DailyOperationWarpingDocument>() { warpingDocument });
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>())).Returns(
                new List<DailyOperationWarpingDocument>() { warpingDocument });

            //Sizing Document Object
            List<BeamId> beamsWarping = new List<BeamId>();
            var beamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            beamsWarping.Add(beamId);

            var sizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(machineDocument.Identity),
                                                                       new OrderId(orderDocument.Identity),
                                                                       beamsWarping,
                                                                       46,
                                                                       400,
                                                                       "PCA 133R",
                                                                       40,
                                                                       4000,
                                                                       40,
                                                                       40,
                                                                       DateTimeOffset.UtcNow,
                                                                       OperationStatus.ONFINISH);
            var sizingHistory = new DailyOperationSizingHistory(new Guid("a85d6656-dc86-40c4-a94a-477c4e1eed66"),
                                                                     new ShiftId(shift.Identity),
                                                                     new OperatorId(operatorDocument.Identity),
                                                                     DateTimeOffset.UtcNow,
                                                                     MachineStatus.ONENTRY,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            sizingDocument.AddDailyOperationSizingHistory(sizingHistory);

            var sizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(beam.Identity),
                                                                             DateTimeOffset.UtcNow,
                                                                             0,
                                                                             1200,
                                                                             40,
                                                                             56,
                                                                             50,
                                                                             40,
                                                                             40,
                                                                             BeamStatus.ROLLEDUP);
            sizingDocument.AddDailyOperationSizingBeamProduct(sizingBeamProduct);

            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingDocumentReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { sizingDocument });

            // Act
            var result = await unitUnderTest.GetById(sizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public async Task GetById_MachineStatusNotEntryOrFinish_ExpectedBehavior()
        {
            // Arrange
            var unitUnderTest = this.CreateDailyOperationSizingQueryHandler();

            //Create Mock Object
            //Supplier Object
            var supplierDocument = new WeavingSupplierDocument(
                Guid.NewGuid(),
                "TS",
                "Test Supplier",
                "999");

            //Fabric Construction Object
            var fabricConstruction = new FabricConstructionDocument(
                Guid.NewGuid(),
                "PolyCotton100 Melintang 33 44 55 PLCTD100 PLCTD100",
                "PolyCotton",
                "Melintang",
                33,
                44,
                55,
                "MMC75",
                "RAYON45",
                100,
                266,
                122);
            mockFabricConstructionRepo.Setup(x => x.Find(It.IsAny<Expression<Func<FabricConstructionReadModel, bool>>>()))
                .Returns(new List<FabricConstructionDocument>() { fabricConstruction });

            //Order Object
            var orderDocument = new OrderDocument(
                Guid.NewGuid(),
                "0002/08-2019",
                DateTime.Now,
                new ConstructionId(fabricConstruction.Identity),
                "MMCXRAYON30",
                new SupplierId(supplierDocument.Identity),
                40,
                40,
                20,
                new SupplierId(supplierDocument.Identity),
                30,
                30,
                40,
                3500,
                new UnitId(14),
                Constants.ONORDER);
            mockOrderDocumentRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OrderReadModel, bool>>>()))
                .Returns(new List<OrderDocument>() { orderDocument });

            //Machine Type Object
            var machineTypeDocument = new MachineTypeDocument(
                new Guid("2C3FECFC-C186-4BAD-B001-D39EE304A17E"),
                "Kawamoto",
                4000,
                "Meter");
            mockMachineTypeRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineTypeReadModel, bool>>>()))
                .Returns(new List<MachineTypeDocument>() { machineTypeDocument });

            //Machine Object
            var machineDocument = new MachineDocument(
                new Guid("2401F96A-40B3-4662-A8BB-4C9A605A8173"),
                "123",
                "Utara",
                new MachineTypeId(machineTypeDocument.Identity),
                new UnitId(11),
                33,
                new UomId(195));
            mockMachineRepo.Setup(x => x.Find(It.IsAny<Expression<Func<MachineDocumentReadModel, bool>>>()))
                .Returns(new List<MachineDocument>() { machineDocument });

            //Operator Object
            var operatorDocument = new OperatorDocument(
                new Guid("BF006F72-857D-4968-AD8F-4745568ACD16"),
                new CoreAccount("5A7C00B2E796C72AA8446601", 0, "Chairul Anam"),
                new UnitId(11),
                "C",
                "AJL",
                "Operator");
            mockOperatorRepo.Setup(x => x.Find(It.IsAny<Expression<Func<OperatorReadModel, bool>>>()))
                .Returns(new List<OperatorDocument>() { operatorDocument });

            //Shift Object
            var shift = new ShiftDocument(
                new Guid("3205F07E-933C-4814-8DED-60FF09EC90B9"),
                "Pagi",
                new TimeSpan(06, 01, 00),
                new TimeSpan(14, 00, 00));
            mockShiftRepo.Setup(x => x.Find(It.IsAny<Expression<Func<ShiftReadModel, bool>>>()))
                .Returns(new List<ShiftDocument>() { shift });

            //Beam Object
            var beam = new BeamDocument(
                new Guid("7C51D68C-9B7B-4FE7-88C3-A1928E810433"),
                "TS56",
                "Sizing",
                6);
            mockBeamRepo.Setup(x => x.Find(It.IsAny<Expression<Func<BeamReadModel, bool>>>()))
                .Returns(new List<BeamDocument>() { beam });

            //Warping Document Object
            //Material Type Object
            var materialType = new MaterialTypeDocument(new Guid("3F851C30-6082-49AE-9BB6-52896B57BEE0"),
                                                             "SPX",
                                                             "Spandex",
                                                             "-");
            //First Warping Document Object
            var warpingDocument = new DailyOperationWarpingDocument(new Guid("73355D43-0EF0-4FD2-A765-B1ACA1004C81"),
                                                                         new OrderId(orderDocument.Identity),
                                                                         40,
                                                                         1,
                                                                         DateTimeOffset.UtcNow,
                                                                         OperationStatus.ONPROCESS);

            var history = new DailyOperationWarpingHistory(new Guid("BB160B68-7724-49A2-9470-0E9F44169812"),
                                                                new ShiftId(shift.Identity),
                                                                new OperatorId(operatorDocument.Identity),
                                                                DateTimeOffset.UtcNow,
                                                                MachineStatus.ONSTART,
                                                                warpingDocument.Identity);
            warpingDocument.WarpingHistories = new List<DailyOperationWarpingHistory>() { history };

            var beamProduct = new DailyOperationWarpingBeamProduct(new Guid("3C5E09E8-0E86-4BB3-890E-057063719F13"),
                                                                        new BeamId(beam.Identity),
                                                                        DateTimeOffset.UtcNow,
                                                                        BeamStatus.ONPROCESS,
                                                                        warpingDocument.Identity);
            warpingDocument.WarpingBeamProducts = new List<DailyOperationWarpingBeamProduct>() { beamProduct };

            //mockDailyOperationWarpingRepo.Setup(x => x.Query).Returns(new List<DailyOperationWarpingDocumentReadModel>().AsQueryable());
            //mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationWarpingDocumentReadModel>>())).Returns(
            //    new List<DailyOperationWarpingDocument>() { warpingDocument });
            mockDailyOperationWarpingRepo.Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationWarpingDocumentReadModel, bool>>>())).Returns(
                new List<DailyOperationWarpingDocument>() { warpingDocument });

            //Sizing Document Object
            List<BeamId> beamsWarping = new List<BeamId>();
            var beamId = new BeamId(new Guid("a955050e-752b-4b96-8e54-f31db9971c66"));
            beamsWarping.Add(beamId);

            var firstSizingDocument = new DailyOperationSizingDocument(new Guid("aa940723-a563-48f1-b2e8-3f70be633950"),
                                                                       new MachineId(machineDocument.Identity),
                                                                       new OrderId(orderDocument.Identity),
                                                                       beamsWarping,
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
                                                                     MachineStatus.ONSTOP,
                                                                     "-",
                                                                     1,
                                                                     1,
                                                                     "S123");
            firstSizingDocument.AddDailyOperationSizingHistory(firstSizingHistory);

            var firstSizingBeamProduct = new DailyOperationSizingBeamProduct(new Guid("bd5403f0-b2a7-4966-9e78-e1622fd7f6d3"),
                                                                             new BeamId(beam.Identity),
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

            mockDailyOperationSizingRepo.Setup(x => x.Query).Returns(new List<DailyOperationSizingDocumentReadModel>().AsQueryable());
            mockDailyOperationSizingRepo.Setup(x => x.Find(It.IsAny<IQueryable<DailyOperationSizingDocumentReadModel>>())).Returns(
                new List<DailyOperationSizingDocument>() { firstSizingDocument });

            // Act
            var result = await unitUnderTest.GetById(firstSizingDocument.Identity);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
