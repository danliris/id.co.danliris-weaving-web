using System;
using Weaving.Domain;
using Weaving.Domain.ValueObjects;

namespace Weaving.Dtos
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
            Id = order.Identity.ToString();
        }

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

        public string Id { get; }
    }
}
