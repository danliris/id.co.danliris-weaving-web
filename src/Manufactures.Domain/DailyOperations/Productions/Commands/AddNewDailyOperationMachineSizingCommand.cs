using FluentValidation;
using Infrastructure.Domain.Commands;
//using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.DailyOperations.Productions.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.DailyOperations.Productions.Commands
{
    public class AddNewDailyOperationMachineSizingCommand : ICommand<DailyOperationMachineSizingDocument>
    {
        [JsonProperty(PropertyName = "Year")]
        public int Year { get; private set; }

        [JsonProperty(PropertyName = "Month")]
        public int Month { get; private set; }

        [JsonProperty(PropertyName = "Day")]
        public int Day { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "EstimatedDetails")]
        public List<AddNewDailyOperationMachineSizingDetailCommand> DailyOperationMachineSizingDetails { get; set; }
    }

    public class AddNewDailyOperationMachineSizingCommandValidator : AbstractValidator<AddNewDailyOperationMachineSizingCommand>
    {
        public AddNewDailyOperationMachineSizingCommandValidator()
        {
            RuleFor(command => command.Year).NotEmpty().WithMessage("Tahun Harus Diisi");
            RuleFor(command => command.Month).NotEmpty().WithMessage("Bulan Harus Diisi");
            RuleFor(command => command.Day).NotEmpty().WithMessage("Hari Harus Diisi");
            RuleFor(command => command.UnitId).NotEmpty().WithMessage("Unit Diisi");
            //RuleFor(command => command.EstimationProducts).NotEmpty();
        }
    }
}
