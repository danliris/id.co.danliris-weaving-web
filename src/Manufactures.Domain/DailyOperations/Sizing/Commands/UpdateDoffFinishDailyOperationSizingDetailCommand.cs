using FluentValidation;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateDoffFinishDailyOperationSizingDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Identity { get; set; }

        [JsonProperty(PropertyName = "BeamDocumentId")]
        public BeamId BeamDocumentId { get; set; }

        //[JsonProperty(PropertyName = "ConstructionDocumentId")]
        //public ConstructionId ConstructionDocumentId { get; set; }

        [JsonProperty(PropertyName = "ShiftDocumentId")]
        public ShiftId ShiftDocumentId { get; set; }

        [JsonProperty(PropertyName = "PIS")]
        public int PIS { get; set; }

        [JsonProperty(PropertyName = "Visco")]
        public string Visco { get; set; }

        [JsonProperty(PropertyName = "Counter")]
        public double Counter { get; set; }
    }
    public class UpdateDoffDailyOperationSizingDetailCommandValidator : AbstractValidator<UpdateDoffFinishDailyOperationSizingDetailCommand>
    {
        public UpdateDoffDailyOperationSizingDetailCommandValidator()
        {
            RuleFor(validator => validator.BeamDocumentId.Value).NotEmpty();
            //RuleFor(validator => validator.ConstructionDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.ShiftDocumentId.Value).NotEmpty();
            RuleFor(validator => validator.PIS).NotEmpty();
            RuleFor(validator => validator.Visco).NotEmpty();
            RuleFor(validator => validator.Counter).NotEmpty();
        }
    }
}
