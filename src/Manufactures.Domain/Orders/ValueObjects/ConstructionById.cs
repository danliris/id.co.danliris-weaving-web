using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Orders.ValueObjects
{
    public class ConstructionById : FabricConstructionDocument
    {
        public string YarnType { get; private set; }

        public ConstructionById(Guid id,
                                 string constructionNumber, 
                                 string yarnType) : base(id, constructionNumber)
        {
            YarnType = yarnType;
        }
    }
}
