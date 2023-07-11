using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Loom.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class DailyOperationLoomMachineCommand : ICommand<DailyOperationLoomMachine>
    {
        public Stream stream { get; set; }

    }
    public class DailyOperationMachineReachingCommandValidator : AbstractValidator<DailyOperationLoomMachineCommand>
    {
        public DailyOperationMachineReachingCommandValidator()
        {
            var commands = new DailyOperationLoomMachineCommand();
            RuleFor(command => command.stream).NotEmpty();
        }
    }
}