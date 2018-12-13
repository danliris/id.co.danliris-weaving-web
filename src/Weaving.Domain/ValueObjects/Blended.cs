using Moonlay.Domain;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Weaving.Domain.ValueObjects
{
    public class Blended : List<float>
    {
        public Blended(IEnumerable<float> items)
        {
            this.AddRange(items);
        }
    }
}
