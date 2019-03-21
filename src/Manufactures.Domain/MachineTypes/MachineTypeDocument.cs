using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.MachineTypes.ReadModels;
using System;

namespace Manufactures.Domain.MachineTypes
{
    public class MachineTypeDocument : AggregateRoot<MachineTypeDocument, 
                                                     MachineTypeReadModel>
    {
        public string TypeName { get; private set; }
        public int Speed { get; private set; }
        public string MachineUnit { get; private set; }
        
        public MachineTypeDocument(Guid id, 
                                   string typeName,
                                   int speed, 
                                   string machineUnit): base(id)
        {
            Identity = id;
            TypeName = typeName;
            Speed = speed;
            MachineUnit = machineUnit;

            this.MarkTransient();

            ReadModel = new MachineTypeReadModel(Identity)
            {
                TypeName = TypeName,
                Speed = Speed,
                MachineUnit = MachineUnit
            };


            ReadModel.AddDomainEvent(new OnAddMachineType(this.Identity));
        }
        
        public MachineTypeDocument(MachineTypeReadModel readModel) : base(readModel)
        {
            this.TypeName = readModel.TypeName;
            this.Speed = readModel.Speed;
            this.MachineUnit = readModel.MachineUnit;
        }

        public void SetTypeName(string value)
        {

            if(value != TypeName)
            {
                TypeName = value;
                ReadModel.TypeName = TypeName;

                MarkModified();
            }
        }

        public void SetMachineSpeed(int value)
        {
            
            if(value != Speed)
            {
                Speed = value;
                ReadModel.Speed = Speed;

                MarkModified();
            }
        }

        public void SetMachineUnit(string value)
        {

            if(value != MachineUnit)
            {

                MachineUnit = value;
                ReadModel.MachineUnit = MachineUnit;

                MarkModified();
            }
        }

        protected override MachineTypeDocument GetEntity()
        {
            return this;
        }
    }
}
