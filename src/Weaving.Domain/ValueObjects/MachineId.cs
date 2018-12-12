using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weaving.Domain.ValueObjects
{
    [JsonConverter(typeof(SingleValueObjectConverter))]
    public class MachineId : SingleValueObject<int>
    {
        public MachineId(int value) : base(value)
        {
        }
    }
}
