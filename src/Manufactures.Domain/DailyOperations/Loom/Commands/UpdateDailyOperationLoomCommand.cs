using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class UpdateDailyOperationLoomCommand 
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; private set; }

        [JsonProperty(PropertyName = "DailyOperationSizingId")]
        public DailyOperationId DailyOperationSizingId { get; set; }

        [JsonProperty(PropertyName = "Detail")]
        public DailyOperationLoomDetailCommand Detail { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class UpdateDailyOperationLoomCommandValidator 
        : AbstractValidator<UpdateDailyOperationLoomCommand>
    {
        public UpdateDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.Detail)
                .SetValidator(new DailyOperationLoomDetailCommandValidator());
        }
    }
}
