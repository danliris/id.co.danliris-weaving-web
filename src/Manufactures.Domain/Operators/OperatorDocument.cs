using Infrastructure.Domain;
using Manufactures.Domain.Operators.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.Operators
{
    public class OperatorDocument 
        : AggregateRoot<OperatorDocument, OperatorReadModel>
    {
        public CoreAccount CoreAccount { get; private set; }
        public UnitId UnitId { get; private set; }
        public string Group { get; private set; }
        public string Assignment { get; private set; }
        public string Type { get; private set; }

        public OperatorDocument(Guid identity, 
                                CoreAccount coreAccount,
                                UnitId unitId,
                                string group,
                                string assignment,
                                string type) : base(identity)
        {
            Identity = identity;
            CoreAccount = coreAccount;
            UnitId = unitId;
            Group = group;
            Assignment = assignment;
            Type = type;

            MarkTransient();

            ReadModel = new OperatorReadModel(Identity)
            {
                CoreAccount = this.CoreAccount.Serialize(),
                UnitId = this.UnitId.Value,
                Group = this.Group,
                Assignment = this.Assignment,
                Type = this.Type
            };
        }

        public OperatorDocument(OperatorReadModel readModel) : base(readModel)
        {
            this.CoreAccount = readModel.CoreAccount.Deserialize<CoreAccount>();
            this.UnitId = readModel.UnitId.HasValue ? new UnitId(readModel.UnitId.Value) : null;
            this.Group = readModel.Group;
            this.Assignment = readModel.Assignment;
            this.Type = readModel.Type;
        }

        public void SetCoreAccount(CoreAccount value)
        {
            if(!value.MongoId.Equals(CoreAccount.MongoId))
            {
                CoreAccount = value;
                ReadModel.CoreAccount = CoreAccount.Serialize();

                MarkModified();
            }
        }

        public void SetUnitId(UnitId unitId)
        {
            if(UnitId.Value != unitId.Value)
            {
                UnitId = unitId;
                ReadModel.UnitId = UnitId.Value;

                MarkModified();
            }
        }

        public void SetGroup(string value)
        {
            if(!value.Equals(Group))
            {
                Group = value;
                ReadModel.Group = Group;

                MarkModified();
            }
        }

        public void SetAssignment(string value)
        {
            if (!value.Equals(Assignment))
            {
                Assignment = value;
                ReadModel.Assignment = Assignment;

                MarkModified();
            }
        }

        public void SetType(string value)
        {
            if (!value.Equals(Type))
            {
                Type = value;
                ReadModel.Type = Type;

                MarkModified();
            }
        }

        protected override OperatorDocument GetEntity()
        {
            return this;
        }
    }
}
