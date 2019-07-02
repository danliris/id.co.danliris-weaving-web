using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Manufactures.Domain.YarnNumbers.Commands
{
    public class UpdateYarnNumberCommand : ICommand<YarnNumberDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }
        
        [JsonProperty(PropertyName = "Number")]
        public int Number { get; set; }

        [JsonProperty(PropertyName = "AdditionalNumber")]
        public int AdditionalNumber { get; private set; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateYarnNumberCommandValidator : AbstractValidator<UpdateYarnNumberCommand>
    {
        public UpdateYarnNumberCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.RingType).NotEmpty();
        }
    }
}
