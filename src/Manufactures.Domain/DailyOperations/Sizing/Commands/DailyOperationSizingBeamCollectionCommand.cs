using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class DailyOperationSizingBeamCollectionCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        //[JsonProperty(PropertyName = "Number")]
        //public string Number { get; set; }

        //[JsonProperty(PropertyName = "Type")]
        //public string Type { get; set; }

        //[JsonProperty(PropertyName = "EmptyWeight")]
        //public double EmptyWeight { get; set; }

        [JsonProperty(PropertyName = "YarnStrands")]
        public double YarnStrands { get; set; }
    }

    public class DailyOperationSizingBeamCollectionCommandValidator
      : AbstractValidator<DailyOperationSizingBeamCollectionCommand>
    {
        public DailyOperationSizingBeamCollectionCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            //RuleFor(validator => validator.Number).NotEmpty();
            //RuleFor(validator => validator.EmptyWeight).NotEmpty();
            RuleFor(validator => validator.YarnStrands).NotEmpty();
        }
    }
}
