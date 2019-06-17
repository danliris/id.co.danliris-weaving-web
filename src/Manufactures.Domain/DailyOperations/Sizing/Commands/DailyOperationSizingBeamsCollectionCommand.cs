using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingBeamsCollectionCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "EmptyWeight")]
        public double EmptyWeight { get; set; }
    }

    public class DailyOperationSizingBeamsCollectionCommandValidator
      : AbstractValidator<DailyOperationSizingBeamsCollectionCommand>
    {
        public DailyOperationSizingBeamsCollectionCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.EmptyWeight).NotEmpty();
        }
    }
}
