using Infrastructure.Domain.Commands;
using Newtonsoft.Json;
using System;

namespace Manufactures.Domain.Defects.FabricDefect.Commands
{
    public class RemoveFabricDefectCommand : ICommand<FabricDefectDocument>
    {
        [JsonProperty(PropertyName = "Id")]
        public Guid Id { get; set; }

        public void SetId(Guid Id)
        {
            this.Id = Id;
        }
    }
}
