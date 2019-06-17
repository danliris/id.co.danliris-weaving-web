using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Sizing.Commands
{
    public class UpdateResumeDailyOperationSizingCommand : ICommand<DailyOperationSizingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "Details")]
        public UpdateResumeDailyOperationSizingDetailCommand Details { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateResumeDailyOperationSizingCommandValidator : AbstractValidator<UpdateResumeDailyOperationSizingCommand>
    {
        public UpdateResumeDailyOperationSizingCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.Details).SetValidator(new UpdateResumeDailyOperationSizingDetailCommandValidator());
        }
    }
}
