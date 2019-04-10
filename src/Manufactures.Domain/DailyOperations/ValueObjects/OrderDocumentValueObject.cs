using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.DailyOperations.ValueObjects
{
    public class OrderDocumentValueObject : ValueObject
    {
        public Guid Identity { get; set; }
        public string OrderNumber { get; set; }
        public string ConstructionNumber { get; set; }
        public string WarpOrigin { get; set; }
        public string WeftOrigin { get; set; }

        public OrderDocumentValueObject(string orderNumber, string constructionNumber, string warpOrigin, string weftOrigin)
        {
            OrderNumber = orderNumber;
            ConstructionNumber = constructionNumber;
            WarpOrigin = warpOrigin;
            WeftOrigin = weftOrigin;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return OrderNumber;
            yield return ConstructionNumber;
            yield return WarpOrigin;
            yield return WeftOrigin;
        }
    }
}
