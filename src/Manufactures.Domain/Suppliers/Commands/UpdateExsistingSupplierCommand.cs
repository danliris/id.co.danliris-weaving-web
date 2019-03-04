using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Suppliers.Commands
{
    public class UpdateExsistingSupplierCommand : ICommand<WeavingSupplierDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "Code")]
        public string Code { get; set; }

        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "CoreSupplierId")]
        public string CoreSupplierId { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateExsistingSupplierCommandValidator : AbstractValidator<UpdateExsistingSupplierCommand>
    {
        public UpdateExsistingSupplierCommandValidator()
        {
            RuleFor(command => command.Code).NotEmpty();
            RuleFor(command => command.Name).NotEmpty();
            RuleFor(command => command.CoreSupplierId).NotEmpty();
        }
    }
}
