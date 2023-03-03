using Infrastructure.Domain;
using Manufactures.Domain.ProductionResults.Entities;
using Manufactures.Domain.ProductionResults.ReadModels;
using Manufactures.Domain.Shared.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Manufactures.Domain.ProductionResults
{
    public class ProductionResultDocument : AggregateRoot<ProductionResultDocument, ProductionResultReadModel>
    {
        public ShiftId ShiftDocumentId { get; private set; }
        public UnitId WeavingUnitId { get; private set; }
        public OrderId OrderDocumentId { get; private set; }
        public DateTimeOffset DateTimeProductionResult { get; private set; }
        public IReadOnlyCollection<ProductionResultProducts> ProductionResultProducts { get; private set; }

        public ProductionResultDocument(Guid id,
                                        ShiftId shiftDocumentId,
                                        UnitId weavingUnitId,
                                        OrderId orderDocumentId,
                                        DateTimeOffset dateTimeProductionResult) : base(id)
        {
            Identity = id;
            ShiftDocumentId = shiftDocumentId;
            WeavingUnitId = weavingUnitId;
            OrderDocumentId = orderDocumentId;
            DateTimeProductionResult = dateTimeProductionResult;
            ProductionResultProducts = new List<ProductionResultProducts>().ToList();

            this.MarkTransient();

            ReadModel = new ProductionResultReadModel(Identity)
            {
                ShiftDocumentId = this.ShiftDocumentId.Value,
                WeavingUnitId = this.WeavingUnitId.Value,
                OrderDocumentId = this.OrderDocumentId.Value,
                DateTimeProductionResult = this.DateTimeProductionResult,
                ProductionResultProducts = this.ProductionResultProducts.ToList()
            };
        }

        public ProductionResultDocument(ProductionResultReadModel readModel) : base(readModel)
        {
            this.ShiftDocumentId =new ShiftId(readModel.ShiftDocumentId);
            this.WeavingUnitId = new UnitId(readModel.WeavingUnitId);
            this.OrderDocumentId = new OrderId(readModel.OrderDocumentId);
            this.DateTimeProductionResult = readModel.DateTimeProductionResult;
            this.ProductionResultProducts = readModel.ProductionResultProducts;
        }

        public void SetShiftDocumentId(ShiftId shiftDocumentId)
        {
            ShiftDocumentId = shiftDocumentId;
            MarkModified();
        }

        public void SetWeavingUnitId(UnitId weavingUnitId)
        {
            WeavingUnitId = weavingUnitId;
            MarkModified();
        }

        public void SetOrderId(OrderId orderDocumentId)
        {
            OrderDocumentId = orderDocumentId;
            MarkModified();
        }

        public void SetDateTimeProductionResult(DateTimeOffset dateTimeProductionResult)
        {
            if (!DateTimeProductionResult.Equals(dateTimeProductionResult))
            {
                DateTimeProductionResult = dateTimeProductionResult;
                ReadModel.DateTimeProductionResult = DateTimeProductionResult;

                MarkModified();
            }
        }

        public void AddProductionResultProduct(ProductionResultProducts productionResultProduct)
        {
            var list = ProductionResultProducts.ToList();
            list.Add(productionResultProduct);
            ProductionResultProducts = list;
            ReadModel.ProductionResultProducts = ProductionResultProducts.ToList();

            MarkModified();
        }

        public void UpdateProductionResultProduct(ProductionResultProducts products)
        {
            var productionResultProducts = ProductionResultProducts.ToList();

            //Get Sizing Detail Update
            var index =
                productionResultProducts
                    .FindIndex(x => x.Identity.Equals(products.Identity));
            var productionResultProduct =
                productionResultProducts
                    .Where(x => x.Identity.Equals(products.Identity))
                    .FirstOrDefault();

            //Update Propertynya
            productionResultProduct.SetMachineId(new MachineId(products.MachineDocumentId));
            productionResultProduct.SetProduction(products.Production);
            productionResultProduct.SetSCMPX(products.SCMPX);
            productionResultProduct.SetEfficiency(products.Efficiency);
            productionResultProduct.SetWeftBrokenThreads(products.WeftBrokenThreads);
            productionResultProduct.SetWarpBrokenThreads(products.WarpBrokenThreads);
            productionResultProduct.SetBrokenThreadsTotal(products.BrokenThreadsTotal);

            productionResultProducts[index] = productionResultProduct;
            ProductionResultProducts = productionResultProducts;
            ReadModel.ProductionResultProducts = productionResultProducts;
            MarkModified();
        }

        public void RemoveProductionResultProduct(Guid identity)
        {
            var product = ProductionResultProducts.Where(o => o.Identity == identity).FirstOrDefault();
            var list = ProductionResultProducts.ToList();

            list.Remove(product);
            ProductionResultProducts = list;
            ReadModel.ProductionResultProducts = ProductionResultProducts.ToList();

            MarkModified();
        }

        protected override ProductionResultDocument GetEntity()
        {
            throw new NotImplementedException();
        }
    }
}
