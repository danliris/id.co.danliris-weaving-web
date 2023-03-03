using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.BrokenCauses.Warping.Commands
{
    public class RemoveWarpingBrokenCauseCommand : ICommand<WarpingBrokenCauseDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
