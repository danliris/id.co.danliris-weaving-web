using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.DailyOperations.Reaching.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching
{
    public class DailyOperationReachingDocument : AggregateRoot<DailyOperationReachingDocument, DailyOperationReachingReadModel>
    {
        public MachineId MachineDocumentId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }
        public ConstructionId ConstructionDocumentId { get; private set; }
        public BeamId SizingBeamId { get; private set; }
        public double PISPieces { get; private set; }
        public string ReachingType { get; private set; }
        public double ReachingWidth { get; private set; }
        public string OperationStatus { get; private set; }
        public List<DailyOperationReachingDetail> ReachingDetails { get; private set; }

        public DailyOperationReachingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, BeamId sizingBeamId, double pisPieces, DailyOperationReachingReachingTypeValueObject reachingType, double reachingWidth, string operationStatus) : base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            SizingBeamId = sizingBeamId;
            PISPieces = pisPieces;
            ReachingType = reachingType.Serialize();
            ReachingWidth = reachingWidth;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationReachingReadModel(Identity)
            {
                MachineDocumentId = MachineDocumentId.Value,
                WeavingUnitId = WeavingUnitId.Value,
                ConstructionDocumentId = ConstructionDocumentId.Value,
                SizingBeamId = SizingBeamId.Value,
                PISPieces = PISPieces,
                ReachingType = ReachingType,
                ReachingWidth = ReachingWidth,
                OperationStatus = OperationStatus,
            };
        }

        public DailyOperationReachingDocument(Guid id, MachineId machineDocumentId, UnitId weavingUnitId, ConstructionId constructionDocumentId, BeamId sizingBeamId, double pisPieces, string operationStatus) : base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            WeavingUnitId = weavingUnitId;
            ConstructionDocumentId = constructionDocumentId;
            SizingBeamId = sizingBeamId;
            PISPieces = pisPieces;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationReachingReadModel(Identity)
            {
                MachineDocumentId = MachineDocumentId.Value,
                WeavingUnitId = WeavingUnitId.Value,
                ConstructionDocumentId = ConstructionDocumentId.Value,
                SizingBeamId = SizingBeamId.Value,
                PISPieces = PISPieces,
                OperationStatus = OperationStatus,
            };
        }

        public DailyOperationReachingDocument(DailyOperationReachingReadModel readModel) : base(readModel)
        {
            MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            WeavingUnitId = readModel.WeavingUnitId.HasValue ? new UnitId(readModel.WeavingUnitId.Value) : null;
            ConstructionDocumentId = readModel.ConstructionDocumentId.HasValue ? new ConstructionId(readModel.ConstructionDocumentId.Value) : null;
            SizingBeamId = readModel.SizingBeamId.HasValue ? new BeamId(readModel.SizingBeamId.Value) : null;
            PISPieces = readModel.PISPieces;
            ReachingType = readModel.ReachingType;
            ReachingWidth = readModel.ReachingWidth;
            OperationStatus = readModel.OperationStatus;
        }

        public void AddDailyOperationReachingDetail(DailyOperationReachingDetail reachingDetail)
        {
            var list = ReachingDetails.ToList();
            list.Add(reachingDetail);
            ReachingDetails = list;
            ReadModel.ReachingDetails = ReachingDetails.ToList();

            MarkModified();
        }

        public void UpdateDailyOperationReachingDetail(DailyOperationReachingDetail detail)
        {
            var reachingDetails = ReachingDetails.ToList();

            //Get Reaching Detail Update
            var index =
                reachingDetails
                    .FindIndex(x => x.Identity.Equals(detail.Identity));
            var reachingDetail =
                reachingDetails
                    .Where(x => x.Identity.Equals(detail.Identity))
                    .FirstOrDefault();

            //Update Detail Properties
            reachingDetail.SetShiftId(new ShiftId(reachingDetail.ShiftDocumentId));
            reachingDetail.SetOperatorDocumentId(new OperatorId(reachingDetail.OperatorDocumentId));
            reachingDetail.SetDateTimeMachine(reachingDetail.DateTimeMachine);
            reachingDetail.SetMachineStatus(reachingDetail.MachineStatus);

            reachingDetails[index] = reachingDetail;
            ReachingDetails = reachingDetails;
            ReadModel.ReachingDetails = reachingDetails;
            MarkModified();
        }

        public void RemoveDailyOperationReachingDetail(Guid identity)
        {
            var detail = ReachingDetails.Where(o => o.Identity == identity).FirstOrDefault();
            var list = ReachingDetails.ToList();

            list.Remove(detail);
            ReachingDetails = list;
            ReadModel.ReachingDetails = ReachingDetails.ToList();

            MarkModified();
        }

        public void SetPISPieces(double pisPieces)
        {
            PISPieces = pisPieces;
            ReadModel.PISPieces = pisPieces;
            MarkModified();
        }

        public void SetReachingType(DailyOperationReachingReachingTypeValueObject reachingType)
        {
            ReachingType = reachingType.Serialize();
            MarkModified();
        }

        public void SetReachingWidth(double reachingWidth)
        {
            ReachingWidth = reachingWidth;
            ReadModel.ReachingWidth = reachingWidth;
            MarkModified();
        }

        public void SetOperationStatus(string operationStatus)
        {
            OperationStatus = operationStatus;
            ReadModel.OperationStatus = operationStatus;
            MarkModified();
        }

        protected override DailyOperationReachingDocument GetEntity()
        {
            return this;
        }
    }
}
