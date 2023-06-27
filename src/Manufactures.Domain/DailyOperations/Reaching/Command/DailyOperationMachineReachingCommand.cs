using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.DailyOperations.Reaching.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Reaching.Command
{
    public class DailyOperationMachineReachingCommand : ICommand<DailyOperationMachineReaching>
    {
        public Stream stream { get; set; }

    }
    public class DailyOperationMachineReachingCommandValidator : AbstractValidator<DailyOperationMachineReachingCommand>
    {
        public DailyOperationMachineReachingCommandValidator()
        {
            var commands = new DailyOperationMachineReachingCommand();
            RuleFor(command => command.stream).NotEmpty();
        }
    }
}
