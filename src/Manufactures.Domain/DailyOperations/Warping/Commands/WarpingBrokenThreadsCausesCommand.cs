using FluentValidation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.Warping.Commands
{
    public class WarpingBrokenThreadsCausesCommand
    {
        [JsonProperty(PropertyName = "WarpingBrokenCauseId")]
        public Guid WarpingBrokenCauseId { get; set; }

        [JsonProperty(PropertyName = "TotalBroken")]
        public int TotalBroken { get; set; }

        public void SetId(Guid Id)
        {
            this.WarpingBrokenCauseId = Id;
        }

        public class WarpingBrokenThreadsCausesCommandValidator : AbstractValidator<WarpingBrokenThreadsCausesCommand>
        {
            public WarpingBrokenThreadsCausesCommandValidator()
            {
                RuleFor(command => command.WarpingBrokenCauseId).NotEmpty();
                RuleFor(command => command.TotalBroken).NotEmpty().WithMessage("Jumlah Putus Harus Diisi");
            }
        }
    }
}
