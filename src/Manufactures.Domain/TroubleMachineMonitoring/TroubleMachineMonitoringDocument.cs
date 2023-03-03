using Infrastructure.Domain;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.TroubleMachineMonitoring.ReadModels;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.TroubleMachineMonitoring
{
    public class TroubleMachineMonitoringDocument : AggregateRoot<TroubleMachineMonitoringDocument, TroubleMachineMonitoringReadModel>
    {
        public DateTimeOffset ContinueDate { get; private set; }
        public DateTimeOffset StopDate { get; private set; }
        public OrderId OrderDocument { get; private set; }
        public string Process { get; private set; }
        public OperatorId OperatorDocument { get; private set; }
        public MachineId MachineDocument { get; private set; }  
        public string Trouble { get; private set; }
        public string Description { get; private set; }
        
        //Main constructor
        public TroubleMachineMonitoringDocument(Guid id,
                                           DateTimeOffset continueDate,
                                           DateTimeOffset stopDate,
                                           OrderId orderId,
                                           string process,
                                           MachineId machineId,
                                           OperatorId operatorId,
                                           string trouble,
                                           string description) : base(id)
        {
            //Instantiate Properties from Parameter Variable
            Identity = id;
            OrderDocument = orderId;
            ContinueDate = continueDate;
            StopDate = stopDate;
            Process = process;
            MachineDocument = machineId;
            OperatorDocument = operatorId;
            Trouble = trouble;
            Description = description;

            this.MarkTransient();

            //Save Object to Database as New One
            ReadModel = new TroubleMachineMonitoringReadModel(Identity)
            {
                StopDate = this.StopDate,
                ContinueDate = this.ContinueDate,
                OperatorDocument = this.OperatorDocument.Value,
                OrderDocument = this.OrderDocument.Value,
                Process = this.Process,
                Trouble = this.Trouble,
                MachineDocument = this.MachineDocument.Value,
                Description = this.Description,                
            };
        }

        public TroubleMachineMonitoringDocument(TroubleMachineMonitoringReadModel readModel) : base(readModel)
        {
            this.OrderDocument = readModel.OrderDocument.HasValue ? new OrderId(readModel.OrderDocument.Value) : null;
            this.StopDate = readModel.StopDate;
            this.Process = readModel.Process;
            this.ContinueDate = readModel.ContinueDate;
            this.MachineDocument = readModel.MachineDocument.HasValue ? new MachineId(readModel.MachineDocument.Value) : null;
            this.OperatorDocument = readModel.OperatorDocument.HasValue ? new OperatorId(readModel.OperatorDocument.Value) : null;
            Trouble = readModel.Trouble;
            Description = readModel.Description;
        }

        public void SetStopDate(DateTimeOffset value)
        {
            StopDate = value;
            ReadModel.StopDate = StopDate;

            MarkModified();
        }

        public void SetContinueDate(DateTimeOffset value)
        {
            ContinueDate = value;
            ReadModel.ContinueDate = ContinueDate;

            MarkModified();
        }

        public void SetOrderId(OrderId order)
        {
            Validator.ThrowIfNull(() => order);

            if (order != OrderDocument)
            {

                OrderDocument = order;
                ReadModel.OrderDocument = OrderDocument.Value;

                MarkModified();
            }
        }

        public void SetMachineId(MachineId value)
        {
            if (MachineDocument != value)
            {
                MachineDocument = value;
                ReadModel.MachineDocument = MachineDocument.Value;

                MarkModified();
            }
        }

        public void SetOperatorId(OperatorId value)
        {
            Validator.ThrowIfNull(() => value);

            if (OperatorDocument  != value)
            {

                OperatorDocument = value;
                ReadModel.OperatorDocument = OperatorDocument.Value;

                MarkModified();
            }
        }

        public void SetProcess(string value)
        {
            Process = value;
            ReadModel.Process = Process;

            MarkModified();
        }

        public void SetTrouble(string value)
        {
            Trouble = value;
            ReadModel.Trouble = Trouble;

            MarkModified();
        }

        public void SetDescription(string value)
        {
            Description = value;
            ReadModel.Description = Description;

            MarkModified();
        }

        protected override TroubleMachineMonitoringDocument GetEntity()
        {
            return this;
        }
    }
}
