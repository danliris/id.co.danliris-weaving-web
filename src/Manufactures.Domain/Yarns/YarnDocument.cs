using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GlobalValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.Yarns.ReadModels;
using Moonlay;
using System;

namespace Manufactures.Domain.Yarns
{
    public class YarnDocument : AggregateRoot<YarnDocument, YarnDocumentReadModel>
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Tags { get; private set; }
        public MaterialTypeId MaterialTypeId { get; private set; }
        public YarnNumberId YarnNumberId { get; private set; }

        public YarnDocument(Guid id,
                            string code,
                            string name,
                            string tags,
                            MaterialTypeId materialTypeId,
                            YarnNumberId yarnNumberId) : base(id)
        {
            Identity = id;
            Code = code;
            Name = name;
            Tags = tags;
            MaterialTypeId = materialTypeId;
            YarnNumberId = yarnNumberId;

            this.MarkTransient();

            ReadModel = new YarnDocumentReadModel(Identity)
            {
                Code = this.Code,
                Name = this.Name,
                Tags = this.Tags,
                MaterialTypeId = this.MaterialTypeId.Value,
                YarnNumberId = this.YarnNumberId.Value
            };

            ReadModel.AddDomainEvent(new OnYarnAddDocument(this.Identity));
        }

        public YarnDocument(YarnDocumentReadModel readModel) : base(readModel)
        {
            this.Code = readModel.Code;
            this.Name = readModel.Name;
            this.Tags = readModel.Tags;
            this.MaterialTypeId = 
                readModel.MaterialTypeId.HasValue ? 
                    new MaterialTypeId(readModel.MaterialTypeId.Value) : null;
            this.YarnNumberId = 
                readModel.YarnNumberId.HasValue ? 
                    new YarnNumberId(readModel.YarnNumberId.Value) : null;
        }

        public void SetCode(string code)
        {
            if (Code != code)
            {
                Code = code;
                ReadModel.Code = Code;

                MarkModified();
            }
        }

        public void SetName(string name)
        {
            if (Name != name)
            {
                Name = name;
                ReadModel.Name = Name;

                MarkModified();
            }
        }

        public void SetTags(string tags)
        {
            if (Tags != tags)
            {
                Tags = tags;
                ReadModel.Tags = Tags;

                MarkModified();
            }
        }

        public void SetMaterialTypeId(MaterialTypeId value)
        {
            if(MaterialTypeId != value)
            {
                MaterialTypeId = value;
                ReadModel.MaterialTypeId = MaterialTypeId.Value;

                MarkModified();
            }
        }

        public void SetYarnNumberId(YarnNumberId value)
        {
            if(YarnNumberId != value)
            {
                YarnNumberId = value;
                ReadModel.YarnNumberId = YarnNumberId.Value;

                MarkModified();
            }
        }

        protected override YarnDocument GetEntity()
        {
            return this;
        }
    }
}
