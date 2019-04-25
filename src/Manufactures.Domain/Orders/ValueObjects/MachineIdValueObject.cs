using Moonlay.Domain;
using Newtonsoft.Json;

namespace Manufactures.Domain.Orders.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MachineIdValueObject : SingleValueObject<int>
    {
        public MachineIdValueObject(int value) : base(value)
        {
        }
    }
}