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
        public Guid? FabricConstructionDocumentId { get; private set; }
    }

    public class ConstructionYarnDetailCommandValidator : AbstractValidator<ConstructionYarnDetailCommand>
    {
        public ConstructionYarnDetailCommandValidator()
        {
            RuleFor(command => command.YarnId).NotNull().NotEmpty().WithMessage("Id Benang Tidak Ditemukan");
            RuleFor(command => command.Quantity).NotNull().NotEmpty().WithMessage("Kuantitas Harus Diisi");
            RuleFor(command => command.Type).NotNull().NotEmpty();
        }
    }
}
