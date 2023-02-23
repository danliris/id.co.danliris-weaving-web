using Infrastructure.Domain;
using Manufactures.Domain.Defects.FabricDefect.ReadModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.Defects.FabricDefect
{
    public class FabricDefectDocument : AggregateRoot<FabricDefectDocument, FabricDefectReadModel>
    {
        public string DefectCode { get; private set; }
        public string DefectType { get; private set; }
        public string DefectCategory { get; private set; }

        public FabricDefectDocument(Guid identity,
                                    string defectCode,
                                    string defectType,
                                    string defectCategory) : base(identity)
        {
            Identity = identity;
            DefectCode = defectCode;
            DefectType = defectType;
            DefectCategory = defectCategory;

            ReadModel = new FabricDefectReadModel(Identity)
            {
                DefectCode = DefectCode,
                DefectType = DefectType,
                DefectCategory = DefectCategory
            };

            MarkTransient();
        }

        public FabricDefectDocument(FabricDefectReadModel readModel) : base(readModel)
        {
            this.DefectCode = readModel.DefectCode;
            this.DefectType = readModel.DefectType;
            this.DefectCategory = readModel.DefectCategory;
        }

        public void SetDefectCode(string value)
        {
            if (DefectCode != value)
            {
                DefectCode = value;
                ReadModel.DefectCode = DefectCode;

                MarkModified();
            }
        }

        public void SetDefectType(string value)
        {
            if (DefectType != value)
            {
                DefectType = value;
                ReadModel.DefectType = DefectType;

                MarkModified();
            }
        }

        public void SetDefectCategory(string value)
        {
            if (DefectCategory != value)
            {
                DefectCategory = value;
                ReadModel.DefectCategory = DefectCategory;

                MarkModified();
            }
        }

        protected override FabricDefectDocument GetEntity()
        {
            return this;
        }
    }
}
