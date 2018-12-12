using Moonlay.Domain;
using Newtonsoft.Json;

namespace Weaving.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class UnitDepartmentId : SingleValueObject<int>
    {
        public UnitDepartmentId(int value) : base(value)
        {

        }
    }
}
