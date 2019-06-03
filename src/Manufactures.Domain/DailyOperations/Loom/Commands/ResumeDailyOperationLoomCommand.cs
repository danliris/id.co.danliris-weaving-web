using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class ResumeDailyOperationLoomCommand 
        : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ResumeDate")]
        public DateTimeOffset ResumeDate { get; set; }

        [JsonProperty(PropertyName = "ResumeTime")]
        public TimeSpan ResumeTime { get; set; }

        [JsonProperty(PropertyName = "ShiftId")]
        public ShiftId ShiftId { get; set; }

        [JsonProperty(PropertyName = "OperatorId")]
        public OperatorId OperatorId { get; set; }

        [JsonProperty(PropertyName = "WarpOrigin")]
        public string WarpOrigin { get; set; }

        [JsonProperty(PropertyName = "WeftOrigin")]
        public string WeftOrigin { get; set; }
    }

    public class ResumeDailyOperationLoomCommandValidator 
        : AbstractValidator<ResumeDailyOperationLoomCommand>
    {
        public ResumeDailyOperationLoomCommandValidator()
        {
            RuleFor(command => command.Id).NotEmpty();
            RuleFor(command => command.ResumeDate).NotEmpty();
            RuleFor(command => command.ResumeTime).NotEmpty();
            RuleFor(command => command.ShiftId.Value).NotEmpty();
            RuleFor(command => command.OperatorId.Value).NotEmpty();
            RuleFor(command => command.WarpOrigin).NotEmpty();
            RuleFor(command => command.WeftOrigin).NotEmpty();
        }
    }
}
