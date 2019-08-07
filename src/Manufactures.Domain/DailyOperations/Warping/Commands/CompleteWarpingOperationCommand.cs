using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class CompleteWarpingOperationCommand
         : ICommand<DailyOperationWarpingDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "DateOperation")]
        public DateTimeOffset DateOperation { get; set; }

        [JsonProperty(PropertyName = "TimeOperation")]
        public String TimeOperation { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }
    }

    public class CompleteWarpingOperationCommandValidator
         : AbstractValidator<CompleteWarpingOperationCommand>
    {
        public CompleteWarpingOperationCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.DateOperation).NotEmpty();
            RuleFor(command => command.TimeOperation).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
        }
    }
}
