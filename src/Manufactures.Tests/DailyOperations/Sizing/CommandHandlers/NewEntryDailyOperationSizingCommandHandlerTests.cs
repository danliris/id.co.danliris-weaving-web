using ExtCore.Data.Abstractions;
using FluentAssertions;
using Manufactures.Application.DailyOperations.Sizing.CommandHandlers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.DailyOperations.Sizing;
using Manufactures.Domain.DailyOperations.Sizing.Commands;
using Manufactures.Domain.DailyOperations.Sizing.Entities;
using Manufactures.Domain.DailyOperations.Sizing.ReadModels;
using Manufactures.Domain.DailyOperations.Sizing.Repositories;
using Manufactures.Domain.DailyOperations.Sizing.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Manufactures.Tests.DailyOperations.Sizing.CommandHandlers
{
    public class NewEntryDailyOperationSizingCommandHandlerTests : IDisposable
    {
        private readonly MockRepository mockRepository;
        private readonly Mock<IStorage> mockStorage;
        private readonly Mock<IDailyOperationSizingRepository>
            mockDailyOperationSizingRepo;

        public NewEntryDailyOperationSizingCommandHandlerTests()
        {
            mockRepository = new MockRepository(MockBehavior.Default);
            mockStorage = mockRepository.Create<IStorage>();
            //mockStorage.Setup(x => x.Save());

            mockDailyOperationSizingRepo = mockRepository.Create<IDailyOperationSizingRepository>();
            mockStorage.Setup(x => x.GetRepository<IDailyOperationSizingRepository>())
                .Returns(mockDailyOperationSizingRepo.Object);
        }

        public void Dispose()
        {
            mockRepository.VerifyAll();
        }

        private NewEntryDailyOperationSizingCommandHandler NewEntryDailyOperationSizingCommandHandler()
        {
            return
                new NewEntryDailyOperationSizingCommandHandler(this.mockStorage.Object);
        }

        /**
         * Test for Create New Entry on Daily Operation Sizing
         * **/
        [Fact]
        public async Task Handle_SameOrderUsed_ThrowError()
        {
            //---ARRANGE---//
            // Set New Entry Command Handler Object
            var createAddNewDailyOperationSizingCommandHandler = this.NewEntryDailyOperationSizingCommandHandler();

            //Create Existing Object & Instantiate It
            var firstExistingWarpingBeamId = new BeamId(Guid.Parse("d1b93878-ed21-4b47-b159-8a51b4423f2e"));
            var secondExistingWarpingBeamId = new BeamId(Guid.Parse("89551ffc-2361-4c0a-bccb-dbc2ae8fd639"));
            var thirdExistingWarpingBeamId = new BeamId(Guid.Parse("0d45a327-5010-46e7-bc61-66637ee39689"));
            var existingListBeamId = new List<BeamId>();
            existingListBeamId.Add(firstExistingWarpingBeamId);
            existingListBeamId.Add(secondExistingWarpingBeamId);
            existingListBeamId.Add(thirdExistingWarpingBeamId);

            var existingSizingDocumentId = Guid.Parse("1b69acf2-6ff8-4911-92cb-090480621eea");
            var existingMachineId = new MachineId(Guid.Parse("edc1994e-6f68-4d7d-8f8c-2e755c1ff157"));
            var existingOrderId = new OrderId(Guid.Parse("e254dc6f-b267-4c8f-aca1-7209059d0652"));
            var existingEmptyWeight = 25;
            var existingYarnStrands = 4000;
            var existingRecipeCode = "PCA 133R";
            var existingNeReal = 40;
            var existingMachineSpeed = 4000;
            var existingTexSQ = "40";
            var existingVisco = "40";
            var existingOperationStatus = OperationStatus.ONPROCESS;

            var existingSizingDocument = new DailyOperationSizingDocument(existingSizingDocumentId,
                                                                          existingMachineId,
                                                                          existingOrderId,
                                                                          existingListBeamId,
                                                                          existingEmptyWeight,
                                                                          existingYarnStrands,
                                                                          existingRecipeCode,
                                                                          existingNeReal,
                                                                          existingMachineSpeed,
                                                                          existingTexSQ,
                                                                          existingVisco,
                                                                          existingOperationStatus);

            var existingSizingDetailId = Guid.Parse("4742a486-8cb7-4611-8f81-d3dc935ac2dd");
            var existingShiftId = new ShiftId(Guid.Parse("aad8276f-dad9-4957-9a71-4f873154f2ca"));
            var existingOperatorId = new OperatorId(Guid.Parse("9444328e-014d-43fa-b077-6c42fcb715b8"));
            var existingDateTimeMachine = DateTimeOffset.UtcNow;
            var existingMachineStatus = MachineStatus.ONCOMPLETE;
            var existingCauses = new DailyOperationSizingCauseValueObject("2", "1");
            var existingInformation = "-";
            var existingSizingBeamNumber = "TS122";

            var existingSizingDetail = new DailyOperationSizingDetail(existingSizingDetailId,
                                                                      existingShiftId,
                                                                      existingOperatorId,
                                                                      existingDateTimeMachine,
                                                                      existingMachineStatus,
                                                                      existingInformation,
                                                                      existingCauses,
                                                                      existingSizingBeamNumber);
            existingSizingDocument.AddDailyOperationSizingDetail(existingSizingDetail);

            //Mocking Setup
            mockDailyOperationSizingRepo
                    .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>() { existingSizingDocument });

            //Create Incoming Object & Instantiate It
            var machineDocumentId = new MachineId(new Guid("723e13e4-c25c-4ff3-bb7a-13c79efbb122"));
            var orderDocumentId = new OrderId(new Guid("e254dc6f-b267-4c8f-aca1-7209059d0652"));

            var firstBeamId = new BeamId(new Guid("a84aa829-ddcb-4496-ad55-92ddf9d8222b"));
            var secondBeamId = new BeamId(new Guid("5900b877-6418-456f-af5f-6b3836518107"));
            var thirdBeamId = new BeamId(new Guid("f06efd73-9bb5-4ae3-961b-601723cf7f7e"));
            var beamsWarping = new List<BeamId>();
            beamsWarping.Add(firstBeamId);
            beamsWarping.Add(secondBeamId);
            beamsWarping.Add(thirdBeamId);

            var shiftId = new ShiftId(Guid.Parse("aad8276f-dad9-4957-9a71-4f873154f2ca"));
            var operatorId = new OperatorId(Guid.Parse("9444328e-014d-43fa-b077-6c42fcb715b8"));
            var preparationDate = DateTimeOffset.UtcNow;
            var preparationTime = new TimeSpan(7);

            var sizingDetail = new NewEntryDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = operatorId,
                ShiftId = shiftId,
                PreparationDate = preparationDate,
                PreparationTime = preparationTime
            };

            var neReal = 2;
            var recipeCode = "PCA 133R";
            var yarnStrands = 63;

            NewEntryDailyOperationSizingCommand request = new NewEntryDailyOperationSizingCommand
            {
                MachineDocumentId = machineDocumentId,
                OrderDocumentId = orderDocumentId,
                BeamsWarping = beamsWarping,
                Details = sizingDetail,
                NeReal = neReal,
                RecipeCode = recipeCode,
                YarnStrands = yarnStrands
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            try
            {
                // Act
                var result = await createAddNewDailyOperationSizingCommandHandler.Handle(request, cancellationToken);
            }
            catch (Exception messageException)
            {
                // Assert
                Assert.Equal("Validation failed: \r\n -- OrderDocument: No. Produksi Sudah Digunakan", messageException.Message);
            }
        }

        [Fact]
        public async Task Handle_NoSameOrderUsed_DataCreated()
        {
            //---ARRANGE---//
            // Set New Entry Command Handler Object
            var createAddNewDailyOperationSizingCommandHandler = this.NewEntryDailyOperationSizingCommandHandler();

            //Mocking Setup
            mockDailyOperationSizingRepo
                    .Setup(x => x.Find(It.IsAny<Expression<Func<DailyOperationSizingReadModel, bool>>>()))
                 .Returns(new List<DailyOperationSizingDocument>());

            //Create Incoming Object & Instantiate It
            var machineDocumentId = new MachineId(Guid.NewGuid());
            var orderDocumentId = new OrderId(Guid.NewGuid());

            var firstBeamId = new BeamId(Guid.NewGuid());
            var secondBeamId = new BeamId(Guid.NewGuid());
            var thirdBeamId = new BeamId(Guid.NewGuid());
            var beamsWarping = new List<BeamId>();
            beamsWarping.Add(firstBeamId);
            beamsWarping.Add(secondBeamId);
            beamsWarping.Add(thirdBeamId);

            var shiftId = new ShiftId(Guid.NewGuid());
            var operatorId = new OperatorId(Guid.NewGuid());
            var preparationDate = DateTimeOffset.UtcNow;
            var preparationTime = new TimeSpan(7);

            var sizingDetail = new NewEntryDailyOperationSizingDetailCommand
            {
                OperatorDocumentId = operatorId,
                ShiftId = shiftId,
                PreparationDate = preparationDate,
                PreparationTime = preparationTime
            };

            var neReal = 2;
            var recipeCode = "PCA 133R";
            var yarnStrands = 63;

            NewEntryDailyOperationSizingCommand request = new NewEntryDailyOperationSizingCommand
            {
                MachineDocumentId = machineDocumentId,
                OrderDocumentId = orderDocumentId,
                BeamsWarping = beamsWarping,
                Details = sizingDetail,
                NeReal = neReal,
                RecipeCode = recipeCode,
                YarnStrands = yarnStrands
            };

            //Set Cancellation Token
            CancellationToken cancellationToken = CancellationToken.None;

            //---ACT---//
            //Instantiate Command Handler
            var result =
                await createAddNewDailyOperationSizingCommandHandler
                    .Handle(request, cancellationToken);

            //---ASSERT---//
            result.Should().NotBeNull();
            result.Identity.Should().NotBeEmpty();
        }
    }
}
