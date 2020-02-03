using FluentValidation;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Estimations.Productions.Entities;
using Manufactures.Domain.Shared.ValueObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.Commands
{
    public class AddNewEstimationCommand : ICommand<EstimatedProductionDocument>
    {
        [JsonProperty(PropertyName = "Year")]
        public int Year { get; private set; }

        [JsonProperty(PropertyName = "Month")]
        public int Month { get; private set; }

        [JsonProperty(PropertyName = "Day")]
        public int Day { get; private set; }

        [JsonProperty(PropertyName = "UnitId")]
        public UnitId UnitId { get; set; }

        [JsonProperty(PropertyName = "EstimationProducts")]
        public List<EstimatedProductionDetail> EstimationDetails { get; set; }
    }

    public class AddNewEstimationCommandValidator : AbstractValidator<AddNewEstimationCommand>
    {
        public AddNewEstimationCommandValidator()
        {
            RuleFor(command => command.Year).NotEmpty().WithMessage("Tahun Harus Diisi");
            RuleFor(command => command.Month).NotEmpty().WithMessage("Bulan Harus Diisi");
            RuleFor(command => command.Day).NotEmpty().WithMessage("Hari Harus Diisi");
            RuleFor(command => command.UnitId).NotEmpty().WithMessage("Unit Diisi");
            //RuleFor(command => command.EstimationProducts).NotEmpty();
        }
    }
}
