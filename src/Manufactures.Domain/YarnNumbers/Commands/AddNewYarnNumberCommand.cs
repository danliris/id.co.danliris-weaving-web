using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.YarnNumbers.Commands
{ 
    public class AddNewYarnNumberCommand : ICommand<YarnNumberDocument>
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Number")]
        public string Number { get; set; }

        [JsonProperty(PropertyName = "AdditionalNumber")]
        public string AdditionalNumber { get; private set; }

        [JsonProperty(PropertyName = "RingType")]
        public string RingType { get; set; }

        [JsonProperty(PropertyName = "Description")]
        public string Description { get; set; }
    }

    public class AddNewYarnNumberCommandValidator : AbstractValidator<AddNewYarnNumberCommand>
    {
        public AddNewYarnNumberCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Number).NotEmpty();
            RuleFor(command => command.RingType).NotEmpty();
        }
    }
}
