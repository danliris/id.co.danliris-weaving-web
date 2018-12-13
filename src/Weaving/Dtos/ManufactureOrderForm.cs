using FluentValidation;
using System;
using Weaving.Domain.ValueObjects;

namespace Weaving.Dtos
{
    public class ManufactureOrderForm
    {
        public DateTimeOffset OrderDate { internal get; set; }

        public UnitDepartmentId UnitDepartmentId { internal get; set; }

        public YarnCodes YarnCodes { internal get; set; }

        public Blended Blended { internal get; set; }

        public MachineId MachineId { internal get; set; }

        public Domain.ManufactureOrder.Status State { internal get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        public string UserId { get; set; }
    }

    public class ManufactureOrderFormValidator : AbstractValidator<ManufactureOrderForm>
    {
        public ManufactureOrderFormValidator()
        {
            RuleFor(r => r.OrderDate).NotNull();
            RuleFor(r => r.MachineId).NotNull();
            RuleFor(r => r.UnitDepartmentId).NotNull();
            RuleFor(r => r.YarnCodes).NotNull();
            RuleFor(r => r.Blended).NotNull();

            RuleFor(r => r.UnitDepartmentId.Value).GreaterThan(0);
        }
    }
}