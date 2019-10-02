using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Defects.YarnDefect.Commands
{
    public class RemoveYarnDefectCommand : ICommand<YarnDefectDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
