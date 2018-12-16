using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.ValueObjects;
using System;

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
            Id = order.Identity;

            LastModifiedDate = order.AuditTrail.ModifiedDate ?? order.AuditTrail.CreatedDate;
            LastModifiedBy = order.AuditTrail.ModifiedBy ?? order.AuditTrail.CreatedBy;
        }

        public Guid Id { get; }

        public DateTimeOffset LastModifiedDate { get; }

        public string LastModifiedBy { get; }

        public DateTimeOffset OrderDate { get; }

        public UnitDepartmentId UnitDepartmentId { get; }

        public YarnCodes YarnCodes { get; }

        public Blended Blended { get; }

        public MachineId MachineId { get; }

        public ManufactureOrder.Status State { get; }

        /// <summary>
        /// Owner
        /// </summary>
        public string UserId { get; }
    }
}