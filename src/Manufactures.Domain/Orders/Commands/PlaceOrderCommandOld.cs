using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Orders.ValueObjects;
using System;

namespace Manufactures.Domain.Orders.Commands
{
    public class PlaceOrderCommandOld : ICommand<ManufactureOrder>
    {
        public DateTimeOffset OrderDate { get; set; }

        public UnitDepartmentId UnitDepartmentId { get; set; }

        public YarnCodes YarnCodes { get; set; }

        public GoodsCompositionId GoodsCompositionId { get; set; }

        public Blended Blended { get; set; }

        public MachineIdValueObject MachineId { get; set; }

        public string UserId { get; set; }
    }

    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommandOld>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(r => r.OrderDate).NotNull();

            RuleFor(r => r.MachineId).NotNull();

            RuleFor(r => r.UnitDepartmentId).NotNull();

            RuleFor(r => r.YarnCodes).NotNull();

            RuleFor(r => r.Blended).NotNull();

            RuleFor(r => r.UserId).NotNull();

            RuleFor(r => r.UnitDepartmentId).NotNull();
        }
    }
}