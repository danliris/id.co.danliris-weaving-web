using Moonlay.Domain;
using System.Collections.Generic;

namespace Manufactures.Domain.Shared.ValueObjects
{
    public class CoreAccount : ValueObject
    {
        public string MongoId { get; private set; }
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CoreAccount(string mongoId, int id, string name)
        {
            MongoId = mongoId;
            Id = id;
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return MongoId;
            yield return Id;
            yield return Name;
        }
    }
}
