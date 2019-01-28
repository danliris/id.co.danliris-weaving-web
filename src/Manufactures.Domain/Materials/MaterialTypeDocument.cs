using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.Materials.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.Materials
{
    public class MaterialTypeDocument : AggregateRoot<MaterialTypeDocument, MaterialTypeReadModel>
    {
        public MaterialTypeDocument(Guid id, 
                            string code, 
                            string name, 
                            string description) : base(id)
        {
            // Validate Properties
            Validator.ThrowIfNullOrEmpty(() => code);
            Validator.ThrowIfNullOrEmpty(() => name);
            Validator.ThrowIfNullOrEmpty(() => description);

            this.MarkTransient();

            Code = code;
            Name = name;
            Description = description;

            ReadModel = new MaterialTypeReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description
            };

            ReadModel.AddDomainEvent(new OnMaterialTypePlace(this.Identity));
        }

        public MaterialTypeDocument(MaterialTypeReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.Description = readModel.Description;
        }

        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

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

        public void SetDescription(string description)
        {
            Validator.ThrowIfNullOrEmpty(() => description);

            if (description != Description)
            {
                Description = description;
                ReadModel.Description = Description;

                MarkModified();
            }
        }

        protected override MaterialTypeDocument GetEntity()
        {
            return this;
        }
    }
}
