using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class Composition : ValueObject
    {
        public Composition(int compositionOfPoly,
                           int compositionOfCotton,
                           int otherComposition)
        {
            CompositionOfPoly = compositionOfPoly;
            CompositionOfCotton = compositionOfCotton;
            OtherComposition = otherComposition;
        }

        public int CompositionOfPoly { get; private set; }
        public int CompositionOfCotton { get; private set; }
        public int OtherComposition { get; set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
