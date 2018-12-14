using System;
using Manufactures.Domain;
using Manufactures.Domain.ValueObjects;

namespace Manufactures.Dtos
{
    public class ManufactureOrderDto
    {
        public ManufactureOrderDto(ManufactureOrder order)
        {
            OrderDate = order.OrderDate;
            UnitDepartmentId = order.UnitDepartmentId;
            YarnCodes = order.YarnCodes;
            Blended = order.Blended;
            MachineId = order.MachineId;
            State = order.State;
            UserId = order.UserId;
            Identity = order.Identity;

            LastModifiedDate = order.AuditTrail.ModifiedDate ?? order.AuditTrail.CreatedDate;
            LastModifiedBy = order.AuditTrail.ModifiedBy ?? order.AuditTrail.CreatedBy;
        }

        public Guid Identity { get; }

        public DateTimeOffset LastModifiedDate { get; }

        public string LastModifiedBy { get; }

        public DateTimeOffset OrderDate { get; internal set; }

        public UnitDepartmentId UnitDepartmentId { get; internal set; }

        public YarnCodes YarnCodes { get; internal set; }

        public Blended Blended { get; internal set; }

        public MachineId MachineId { get; internal set; }

        public Domain.ManufactureOrder.Status State { get; internal set; }

        /// <summary>
        /// Owner
        /// </summary>
        public string UserId { get; internal set; }

    }
}
