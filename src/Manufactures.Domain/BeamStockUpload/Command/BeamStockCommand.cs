using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.BeamStockUpload.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Manufactures.Domain.BeamStockUpload.Command
{
    public class BeamStockCommand : ICommand<BeamStock>
    {
        public Stream stream { get; set; }

    }
    public class BeamStockCommandValidator : AbstractValidator<BeamStockCommand>
    {
        public BeamStockCommandValidator()
        {
            var commands = new BeamStockCommand();
            RuleFor(command => command.stream).NotEmpty();
        }
    }
}