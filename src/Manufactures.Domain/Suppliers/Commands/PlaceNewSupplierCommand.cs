using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class PlaceNewSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "CoreSupplierId")]
        public string CoreSupplierId { get; set; }
    }

    public class PlaceNewSupplierCommandValidator : AbstractValidator<PlaceNewSupplierCommand>
    {
        public PlaceNewSupplierCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.CoreSupplierId).NotEmpty();
        }
    }
}
