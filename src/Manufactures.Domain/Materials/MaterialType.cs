using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Materials.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.Materials
{
    public class MaterialType : AggregateRoot<MaterialType, MaterialTypeReadModel>
    {
        public MaterialType(Guid id, string code, string name) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => name);

            this.MarkTransient();

            Code = code;
            Name = name;

            ReadModel = new MaterialTypeReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name
            };

            ReadModel.AddDomainEvent(new OnMaterialTypePlace(this.Identity));
        }

        public MaterialType(MaterialTypeReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }

        public void SetCode(string code)
        {
            Validator.ThrowIfNullOrEmpty(() => code);

            if (code != Code)
            {
                Code = code;
                ReadModel.Code = Code;

                MarkModified();
            }
        }

        public void SetName(string name)
        {
            Validator.ThrowIfNullOrEmpty(() => name);

            if (name != Name)
            {
                Name = name;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        protected override MaterialType GetEntity()
        {
            return this;
        }
    }
}
