using FluentValidation;
using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Loom.Commands
{
    public class ProduceGreigeDailyOperationLoomCommand : ICommand<DailyOperationLoomDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeBeamUsedId")]
        public Guid ProduceGreigeBeamUsedId { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeBeamUsedNumber")]
        public string ProduceGreigeBeamUsedNumber { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeTyingOperatorDocumentId")]
        public Guid ProduceGreigeTyingOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeTyingOperatorName")]
        public string ProduceGreigeTyingOperatorName { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeLoomOperatorDocumentId")]
        public Guid ProduceGreigeLoomOperatorDocumentId { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeDate")]
        public DateTimeOffset ProduceGreigeDate { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeTime")]
        public TimeSpan ProduceGreigeTime { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeShiftDocumentId")]
        public Guid ProduceGreigeDocumentId { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeCounter")]
        public double ProduceGreigeCounter { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeUomDocumentId")]
        public Guid ProduceGreigeUomDocumentId { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeUomUnit")]
        public string ProduceGreigeUomUnit{ get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeMachineSpeed")]
        public double ProduceGreigeMachineSpeed { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeSCMPX")]
        public double ProduceGreigeSCMPX{ get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeEfficiency")]
        public double ProduceGreigeEfficiency{ get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeF")]
        public double ProduceGreigeF { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeW")]
        public double ProduceGreigeW { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeL")]
        public double ProduceGreigeL { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeT")]
        public double ProduceGreigeT { get; set; }

        [JsonProperty(PropertyName = "ProduceGreigeIsCompletedProduction")]
        public bool ProduceGreigeIsCompletedProduction { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }

    public class ProduceGreigeDailyOperationLoomCommandValidator : AbstractValidator<ProduceGreigeDailyOperationLoomCommand>
    {
        public ProduceGreigeDailyOperationLoomCommandValidator()
        {
            RuleFor(validator => validator.Id).NotEmpty();
            RuleFor(validator => validator.ProduceGreigeBeamUsedId).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.ProduceGreigeBeamUsedNumber).NotEmpty().WithMessage("No. Beam Harus Diisi");
            RuleFor(validator => validator.ProduceGreigeLoomOperatorDocumentId).NotEmpty().WithMessage("Operator Harus Diisi");
            RuleFor(validator => validator.ProduceGreigeDate).NotEmpty().WithMessage("Tanggal Mulai Harus Diisi");
            RuleFor(validator => validator.ProduceGreigeTime).NotEmpty().WithMessage("Waktu Mulai Harus Diisi");
            RuleFor(validator => validator.ProduceGreigeDocumentId).NotEmpty().WithMessage("Shift Tidak Boleh Kosong");
        }
    }
}
