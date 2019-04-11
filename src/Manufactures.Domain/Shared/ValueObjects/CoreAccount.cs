using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class CoreAccount : ValueObject
    {
        public string MongoId { get; private set; }
        public int Id { get; private set; }
        
        public CoreAccount(string mongoId)
        {
            MongoId = mongoId;
        }

        public CoreAccount(string mongoId, int id)
        {
            MongoId = mongoId;
            Id = id;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MongoId;
            yield return Id;
        }
    }
}
