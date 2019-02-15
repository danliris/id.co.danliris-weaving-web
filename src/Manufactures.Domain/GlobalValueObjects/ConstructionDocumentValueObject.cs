using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GlobalValueObjects
{
    public class ConstructionDocumentValueObject
    {
        public Guid Identity { get; }
        public string ConstructionNumber { get; }
        public double TotalYarn { get; }

        public ConstructionDocumentValueObject(Guid identity,
                                               string constructionNumber,
                                               double totalYarn)
        {
            Identity = identity;
            ConstructionNumber = constructionNumber;
            TotalYarn = totalYarn;
        }
    }
}
