using Infrastructure.Domain;
using Manufactures.Domain.FabricConstructions.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.FabricConstructions.Entity
{
    public class ConstructionYarnDetail : AggregateRoot<ConstructionYarnDetail, ConstructionYarnDetailReadModel>
    {

        public YarnId YarnId { get; private set; }
        public double Quantity { get; private set; }
        public string Information { get; private set; }
        public string Type { get; private set; }
        public Guid FabricConstructionDocumentId { get; set; }

        public ConstructionYarnDetail(Guid id,
                                      YarnId yarnId,
                                      double quantity,
                                      string information,
                                      string type,
                                      Guid fabricConstructionDocumentId) : base(id)
        {
            Identity = id;
            YarnId = yarnId;
            Quantity = quantity;
            Information = information;
            Type = type;
            FabricConstructionDocumentId = fabricConstructionDocumentId;

            MarkTransient();

            ReadModel = new ConstructionYarnDetailReadModel(Identity)
            {
                YarnId = YarnId.Value,
                Quantity = Quantity,
                Information = Information,
                Type = Type,
                FabricConstructionDocumentId = FabricConstructionDocumentId
            };
        }

        public ConstructionYarnDetail(ConstructionYarnDetailReadModel readModel) : base(readModel)
        {
            YarnId = new YarnId(readModel.YarnId);
            Quantity = readModel.Quantity;
            Information = readModel.Information;
            Type = readModel.Type;
        }

        public void SetYarnId(YarnId yarnId)
        {
            Validator.ThrowIfNull(() => yarnId);

            if (yarnId != YarnId)
            {
                YarnId = yarnId;
                ReadModel.YarnId = yarnId.Value;

                MarkModified();
            }
        }

        public void SetQuantity(double quantity)
        {
            if (quantity != Quantity)
            {
                Quantity = quantity;
                ReadModel.Quantity = quantity;

                MarkModified();
            }
        }

        public void SetInformation(string information)
        {
            Validator.ThrowIfNull(() => information);

            if (information != Information)
            {
                Information = information;
                ReadModel.Information = information;

                MarkModified();
            }
        }

        public void SetType(string type)
        {
            Validator.ThrowIfNull(() => type);

            if (type != Type)
            {
                Type = type;
                ReadModel.Type = type;

                MarkModified();
            }
        }

        protected override ConstructionYarnDetail GetEntity()
        {
            return this;
        }
    }
}
