using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.OperationalMachinesPlanning.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;

namespace Manufactures.Domain.OperationalMachinesPlanning
{
    public class MachinesPlanningDocument : AggregateRoot<MachinesPlanningDocument,
                                                        MachinesPlanningReadModel>
    {
        public string Area { get; internal set; }
        public string Blok { get; internal set; }
        public string BlokKaizen { get; internal set; }
        public UnitId UnitDepartementId { get; internal set; }
        public MachineId MachineId { get; internal set; }
        public UserId UserMaintenanceId { get; internal set; }
        public UserId UserOperatorId { get; internal set; }

        public MachinesPlanningDocument(Guid identity,
                                      string area,
                                      string blok,
                                      string blokKaizen,
                                      UnitId unitDepartmentId,
                                      MachineId machineId,
                                      UserId userMaintenanceId,
                                      UserId userOperatorId) : base(identity)
        {
            Identity = identity;
            Area = area;
            Blok = blok;
            BlokKaizen = blokKaizen;
            UnitDepartementId = unitDepartmentId;
            MachineId = machineId;
            UserMaintenanceId = userMaintenanceId;
            UserOperatorId = userOperatorId;

            this.MarkTransient();

            ReadModel = new MachinesPlanningReadModel(this.Identity)
            {
                Area = this.Area,
                Blok = this.Blok,
                BlokKaizen = this.BlokKaizen,
                UnitDepartementId = this.UnitDepartementId.Value,
                MachineId = this.MachineId.Value,
                UserMaintenanceId = this.UserMaintenanceId.Value,
                UserOperatorId = this.UserOperatorId.Value
            };

            ReadModel.AddDomainEvent(new OnAddEnginePlanning(this.Identity));
        }

        public MachinesPlanningDocument(MachinesPlanningReadModel readModel) : base(readModel)
        {
            this.Area = readModel.Area;
            this.Blok = readModel.Blok;
            this.BlokKaizen = readModel.BlokKaizen;
            this.UnitDepartementId = readModel.UnitDepartementId.HasValue ? new UnitId(readModel.UnitDepartementId.Value) : null;
            this.MachineId = readModel.MachineId.HasValue ? new MachineId(readModel.MachineId.Value) : null;
            this.UserMaintenanceId = readModel.UserMaintenanceId.HasValue ? new UserId(readModel.UserMaintenanceId.Value) : null;
            this.UserOperatorId = readModel.UserOperatorId.HasValue ? new UserId(readModel.UserOperatorId.Value) : null;
        }

        public void ChangeArea(string value)
        {
            if (Area != value)
            {
                Area = value;
                ReadModel.Area = Area;

                this.MarkModified();
            }
        }

        public void ChangeBlok(string value)
        {
            if (Blok != value)
            {
                Blok = value;
                ReadModel.Blok = Blok;

                this.MarkModified();
            }
        }

        public void ChangeBlokKaizen(string value)
        {
            if (BlokKaizen != value)
            {
                BlokKaizen = value;
                ReadModel.BlokKaizen = BlokKaizen;

                this.MarkModified();
            }
        }

        public void ChangeUnitDepartmentId(int value)
        {
            if (UnitDepartementId.Value != value)
            {
                UnitDepartementId = new UnitId(value);
                ReadModel.UnitDepartementId = UnitDepartementId.Value;

                this.MarkModified();
            }
        }

        public void ChangeMachineId(Guid value)
        {
            if(MachineId.Value != value)
            {
                MachineId = new MachineId(value);
                ReadModel.MachineId = MachineId.Value;

                this.MarkModified();
            }
        }

        public void ChangeUserMaintenanceId(int value)
        {
            if (UserMaintenanceId.Value != value)
            {
                UserMaintenanceId = new UserId(value);
                ReadModel.UserMaintenanceId = UserMaintenanceId.Value;

                this.MarkModified();
            }
        }

        public void ChangeUserOperatorId(int value)
        {
            if (UserOperatorId.Value != value)
            {
                UserOperatorId = new UserId(value);
                ReadModel.UserOperatorId = UserOperatorId.Value;

                this.MarkModified();
            }
        }

        protected override MachinesPlanningDocument GetEntity()
        {
            return this;
        }
    }
}
