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

        [JsonProperty(PropertyName = "SizingBeamDocuments")]
        public UpdateStartDailyOperationSizingBeamDocumentCommand SizingBeamDocuments { get; set; }

        [JsonProperty(PropertyName = "SizingDetails")]
        public UpdateStartDailyOperationSizingDetailCommand SizingDetails { get; set; }

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
            RuleFor(validator => validator.SizingBeamDocuments).SetValidator(new UpdateStartDailyOperationSizingBeamDocumentCommandValidator());
            RuleFor(validator => validator.SizingDetails).SetValidator(new UpdateStartDailyOperationSizingDetailCommandValidator());
        }
    }
}
