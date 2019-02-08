using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class ConstructionDocument : ValueObject
    {
        public Guid Identity { get; private set; }
        public string ConstructionNumber { get; private set; }
        public double TotalYarn { get; private set; }

        public ConstructionDocument(Guid identity,
                                    string constructionNumber,
                                    double totalYarn)
        {
            Identity = identity;
            ConstructionNumber = constructionNumber;
            TotalYarn = totalYarn;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return ConstructionNumber;
            yield return TotalYarn;
        }
    }
}
