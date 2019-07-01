using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateStartDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "NeReal")]
        public double NeReal { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public UpdateStartDailyOperationSizingDetailCommand Details { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class AddNewDailyOperationSizingCommandValidator : AbstractValidator<UpdateStartDailyOperationSizingCommand>
    {
        public AddNewDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.NeReal).NotEmpty();
            RuleFor(validator => validator.Details).SetValidator(new UpdateStartDailyOperationSizingDetailCommandValidator());
        }
    }
}
