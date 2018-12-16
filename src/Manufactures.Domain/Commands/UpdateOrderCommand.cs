using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.ValueObjects;
using System;

namespace Manufactures.Domain.Commands
{
    public class UpdateOrderCommand : ICommand<ManufactureOrder>
    {
        public void SetId(Guid id) { Id = id; }
        public Guid Id { get; private set; }

        public UnitDepartmentId UnitDepartmentId { get; set; }

        public YarnCodes YarnCodes { get; set; }

        public Blended Blended { get; set; }

        public MachineId MachineId { get; set; }

        public string UserId { get; set; }
    }

    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(r => r.MachineId).NotNull();

            RuleFor(r => r.UnitDepartmentId).NotNull();

            RuleFor(r => r.YarnCodes).NotNull();

            RuleFor(r => r.Blended).NotNull();

            RuleFor(r => r.UserId).NotNull();

            RuleFor(r => r.UnitDepartmentId).NotNull();
        }
    }
}
