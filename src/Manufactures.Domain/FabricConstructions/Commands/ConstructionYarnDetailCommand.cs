using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.Commands
{
    public class ConstructionYarnDetailCommand
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid? Id { get; private set; }

        [JsonProperty(PropertyName = "YarnId")]
        public Guid YarnId { get; private set; }

        [JsonProperty(PropertyName = "Quantity")]
        public double Quantity { get; private set; }

        [JsonProperty(PropertyName = "Information")]
        public string Information { get; private set; }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; private set; }

        [JsonProperty(PropertyName = "FabricConstructionDocumentId")]
        public Guid FabricConstructionDocumentId { get; private set; }
    }

    public class ConstructionYarnDetailCommandValidator : AbstractValidator<ConstructionYarnDetailCommand>
    {
        public ConstructionYarnDetailCommandValidator()
        {
            RuleFor(command => command.YarnId).NotEmpty();
            RuleFor(command => command.Quantity).NotEmpty();
            RuleFor(command => command.Type);
        }
    }
}
