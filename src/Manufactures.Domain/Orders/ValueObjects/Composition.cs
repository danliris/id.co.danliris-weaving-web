using Moonlay.Domain;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class Composition : ValueObject
    {
        [JsonProperty(PropertyName = "CompositionOfPoly")]
        public int CompositionOfPoly { get; private set; }

        [JsonProperty(PropertyName = "CompositionOfCotton")]
        public int CompositionOfCotton { get; private set; }

        [JsonProperty(PropertyName = "OtherComposition")]
        public int OtherComposition { get; set; }

        public Composition(int compositionOfPoly,
                           int compositionOfCotton,
                           int otherComposition)
        {
            CompositionOfPoly = compositionOfPoly;
            CompositionOfCotton = compositionOfCotton;
            OtherComposition = otherComposition;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return CompositionOfPoly;
            yield return CompositionOfCotton;
            yield return OtherComposition;
        }
    }
}
