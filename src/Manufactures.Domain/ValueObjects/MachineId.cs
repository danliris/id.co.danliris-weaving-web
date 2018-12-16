using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MachineId : SingleValueObject<int>
    {
        public MachineId(int value) : base(value)
        {
        }
    }
}