using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class SizingBeamCollectionCommand
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

    public class SizingBeamCollectionCommandValidator
      : AbstractValidator<SizingBeamCollectionCommand>
    {
        public SizingBeamCollectionCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.Number).NotEmpty();
            RuleFor(validator => validator.EmptyWeight).NotEmpty();
        }
    }
}
