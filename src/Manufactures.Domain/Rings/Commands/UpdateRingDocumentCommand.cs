using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Rings.Commands
{
    public class UpdateRingDocumentCommand : ICommand<RingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateRingDocumentCommandValidator : AbstractValidator<UpdateRingDocumentCommand>
    {
        public UpdateRingDocumentCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.RingType).NotEmpty();
        }
    }
}
