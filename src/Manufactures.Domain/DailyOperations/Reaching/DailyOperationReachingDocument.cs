using Infrastructure.Domain;
using Manufactures.Domain.DailyOperations.Reaching.ReadModels;
using Manufactures.Domain.Events;
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
        public OrderId OrderDocumentId { get; private set; }
        public BeamId SizingBeamId { get; private set; }
        public string ReachingInTypeInput { get; private set; }
        public string ReachingInTypeOutput { get; private set; }
        public double ReachingInWidth { get; private set; }
        public int CombEdgeStitching { get; private set; }
        public int CombNumber { get; private set; }
        public double CombWidth { get; private set; }
        public string OperationStatus { get; private set; }

        public DailyOperationReachingDocument(Guid id, 
                                              MachineId machineDocumentId, 
                                              OrderId orderDocumentId, 
                                              BeamId sizingBeamId, 
                                              string reachingInTypeInput, 
                                              string reachingInTypeOutput, 
                                              double reachingInWidth, 
                                              int combEdgeStitching, 
                                              int combNumber, 
                                              double combWidth,
                                              string operationStatus) : base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            OrderDocumentId = orderDocumentId;
            SizingBeamId = sizingBeamId;
            ReachingInTypeInput = reachingInTypeInput;
            ReachingInTypeOutput = reachingInTypeOutput;
            ReachingInWidth = reachingInWidth;
            CombEdgeStitching = combEdgeStitching;
            CombNumber = combNumber;
            CombWidth = combWidth;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationReachingReadModel(Identity)
            {
                MachineDocumentId = MachineDocumentId.Value,
                OrderDocumentId = OrderDocumentId.Value,
                SizingBeamId = SizingBeamId.Value,
                ReachingInTypeInput = ReachingInTypeInput,
                ReachingInTypeOutput = ReachingInTypeOutput,
                ReachingInWidth = ReachingInWidth,
                CombEdgeStitching = CombEdgeStitching,
                CombNumber = CombNumber,
                CombWidth = CombWidth,
                OperationStatus = OperationStatus
            };

            ReadModel.AddDomainEvent(new OnAddDailyOperationReachingDocument(Identity));
        }

        public DailyOperationReachingDocument(Guid id, 
                                              MachineId machineDocumentId, 
                                              OrderId orderDocumentId, 
                                              BeamId sizingBeamId,
                                              string reachingInTypeInput,
                                              string reachingInTypeOutput,
                                              double reachingInWidth, 
                                              string operationStatus) : base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            OrderDocumentId = orderDocumentId;
            SizingBeamId = sizingBeamId;
            ReachingInTypeInput = reachingInTypeInput;
            ReachingInTypeOutput = reachingInTypeOutput;
            ReachingInWidth = reachingInWidth;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationReachingReadModel(Identity)
            {
                MachineDocumentId = MachineDocumentId.Value,
                OrderDocumentId = OrderDocumentId.Value,
                SizingBeamId = SizingBeamId.Value,
                ReachingInTypeInput = ReachingInTypeInput,
                ReachingInTypeOutput = ReachingInTypeOutput,
                ReachingInWidth = ReachingInWidth,
                OperationStatus = OperationStatus,
            };
            ReadModel.AddDomainEvent(new OnAddDailyOperationReachingDocument(Identity));
        }

        public DailyOperationReachingDocument(Guid id, MachineId machineDocumentId, OrderId orderDocumentId, BeamId sizingBeamId, string operationStatus) : base(id)
        {
            Identity = id;
            MachineDocumentId = machineDocumentId;
            OrderDocumentId = orderDocumentId;
            SizingBeamId = sizingBeamId;
            OperationStatus = operationStatus;

            this.MarkTransient();

            ReadModel = new DailyOperationReachingReadModel(Identity)
            {
                MachineDocumentId = MachineDocumentId.Value,
                OrderDocumentId = OrderDocumentId.Value,
                SizingBeamId = SizingBeamId.Value,
                OperationStatus = OperationStatus,
            };
            ReadModel.AddDomainEvent(new OnAddDailyOperationReachingDocument(Identity));
        }

        public DailyOperationReachingDocument(DailyOperationReachingReadModel readModel) : base(readModel)
        {
            MachineDocumentId = readModel.MachineDocumentId.HasValue ? new MachineId(readModel.MachineDocumentId.Value) : null;
            OrderDocumentId = readModel.OrderDocumentId.HasValue ? new OrderId(readModel.OrderDocumentId.Value) : null;
            SizingBeamId = readModel.SizingBeamId.HasValue ? new BeamId(readModel.SizingBeamId.Value) : null;
            ReachingInTypeInput = readModel.ReachingInTypeInput;
            ReachingInTypeOutput = readModel.ReachingInTypeOutput;
            ReachingInWidth = readModel.ReachingInWidth;
            CombEdgeStitching = readModel.CombEdgeStitching;
            CombNumber = readModel.CombNumber;
            CombWidth = readModel.CombWidth;
            OperationStatus = readModel.OperationStatus;
        }

        //public void AddDailyOperationReachingHistory(DailyOperationReachingHistory history)
        //{
        //    var list = ReachingHistories.ToList();
        //    list.Add(history);
        //    ReachingHistories = list;
        //    ReadModel.ReachingHistories = ReachingHistories.ToList();

        //    MarkModified();
        //}

        //public void UpdateDailyOperationReachingHistory(DailyOperationReachingHistory history)
        //{
        //    var reachingHistories = ReachingHistories.ToList();

        //    //Get Reaching History Update
        //    var index =
        //        reachingHistories
        //            .FindIndex(x => x.Identity.Equals(history.Identity));
        //    var reachingHistory =
        //        reachingHistories
        //            .Where(x => x.Identity.Equals(history.Identity))
        //            .FirstOrDefault();

        //    //Update History Properties
        //    reachingHistory.SetShiftId(new ShiftId(history.ShiftDocumentId));
        //    reachingHistory.SetOperatorDocumentId(new OperatorId(history.OperatorDocumentId));
        //    reachingHistory.SetDateTimeMachine(history.DateTimeMachine);
        //    reachingHistory.SetMachineStatus(history.MachineStatus);

        //    reachingHistories[index] = reachingHistory;
        //    ReachingHistories = reachingHistories;
        //    ReadModel.ReachingHistories = reachingHistories;
        //    MarkModified();
        //}

        //public void RemoveDailyOperationReachingHistory(Guid identity)
        //{
        //    var history = ReachingHistories.Where(o => o.Identity == identity).FirstOrDefault();
        //    var list = ReachingHistories.ToList();

        //    list.Remove(history);
        //    ReachingHistories = list;
        //    ReadModel.ReachingHistories = ReachingHistories.ToList();

        //    MarkModified();
        //}

        public void SetReachingInTypeInput(string reachingInTypeInput)
        {
            if(ReachingInTypeInput != reachingInTypeInput)
            {
                ReachingInTypeInput = reachingInTypeInput;
                ReadModel.ReachingInTypeInput = ReachingInTypeInput;
                MarkModified();
            }
            
        }

        public void SetReachingInTypeOutput(string reachingInTypeOutput)
        {
            if(ReachingInTypeOutput != reachingInTypeOutput)
            {
                ReachingInTypeOutput = reachingInTypeOutput;
                ReadModel.ReachingInTypeOutput = ReachingInTypeOutput;
                MarkModified();
            }
            
        }

        public void SetReachingInWidth(double reachingInWidth)
        {
            if(ReachingInWidth != reachingInWidth)
            {
                ReachingInWidth = reachingInWidth;
                ReadModel.ReachingInWidth = ReachingInWidth;
                MarkModified();
            }
            
        }

        public void SetCombEdgeStitching(int combEdgeStitching)
        {
            if(CombEdgeStitching != combEdgeStitching)
            {
                CombEdgeStitching = combEdgeStitching;
                ReadModel.CombEdgeStitching = CombEdgeStitching;
                MarkModified();
            }
            
        }

        public void SetCombNumber(int combNumber)
        {
            if(CombNumber != combNumber)
            {
                CombNumber = combNumber;
                ReadModel.CombNumber = CombNumber;
                MarkModified();
            }
            
        }

        public void SetCombWidth(double combWidth)
        {
            if(CombWidth != combWidth)
            {
                CombWidth = combWidth;
                ReadModel.CombWidth = CombWidth;
                MarkModified();
            }
            
        }

        public void SetOperationStatus(string operationStatus)
        {
            if(OperationStatus != operationStatus)
            {
                OperationStatus = operationStatus;
                ReadModel.OperationStatus = OperationStatus;
                MarkModified();
            }
            
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override DailyOperationReachingDocument GetEntity()
        {
            return this;
        }
    }
}
