using Moonlay.Domain;
using System;
using System.Collections.Generic;

namespace Manufactures.Domain.Estimations.Productions.ValueObjects
{
    public class OrderDocumentValueObject : ValueObject
    {
        public OrderDocumentValueObject(Guid identity, 
                             string orderNumber, 
                             int allGrade,
                             ConstructionDocument construction,
                             DateTimeOffset dateOrdered)
        {
            Identity = identity;
            OrderNumber = orderNumber;
            AllGrade = allGrade;
            Construction = construction;
            DateOrdered = dateOrdered;
        }

        public Guid Identity { get; private set; }
        public string OrderNumber { get; private set; }
        public int AllGrade { get; private set; }
        public ConstructionDocument Construction { get; private set; }
        public DateTimeOffset DateOrdered { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Identity;
            yield return OrderNumber;
            yield return AllGrade;
            yield return Construction;
            yield return DateOrdered;
        }
    }
}
