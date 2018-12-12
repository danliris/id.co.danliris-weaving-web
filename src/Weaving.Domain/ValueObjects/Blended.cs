using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Weaving.Domain.ValueObjects
{
    public class Blended : ValueObject
    {
        public Blended(List<float> items)
        {
            Items = items;
        }

        public List<float> Items { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Items;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
