using Barebone.Controllers;
using Manufactures.Application.Helpers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Orders.ValueObjects;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Dtos.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Microsoft.EntityFrameworkCore;
using Manufactures.Domain.Yarns.Repositories;
using Manufactures.Helpers.PdfTemplates;
using Manufactures.Application.Orders.DataTransferObjects.OrderReport;
using Manufactures.Domain.Orders.Queries.OrderReport;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/orders")]
    [ApiController]
    [Authorize]
    public class OrderDocumentController : ControllerApiBase
    {
        private readonly IWeavingOrderDocumentRepository
                                               _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository
                                               _constructionDocumentRepository;
        private readonly IEstimationProductRepository
                                               _estimationProductRepository;
        private readonly IYarnDocumentRepository
                                               _yarnDocumentRepository;

        private readonly IOrderReportQuery<OrderReportListDto> _orderProductionReportQuery;

        public OrderDocumentController(IServiceProvider serviceProvider,
                                       IWorkContext workContext,
                                       IOrderReportQuery<OrderReportListDto> orderProductionReportQuery) : base(serviceProvider)
        {
            _weavingOrderDocumentRepository =
                this.Storage.GetRepository<IWeavingOrderDocumentRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _estimationProductRepository =
                this.Storage.GetRepository<IEstimationProductRepository>();
            _yarnDocumentRepository =
                this.Storage.GetRepository<IYarnDocumentRepository>();

            _orderProductionReportQuery = orderProductionReportQuery ?? throw new ArgumentNullException(nameof(orderProductionReportQuery));
        }

        [HttpGet]
        [Route("request-order-number")]
        public async Task<IActionResult> GetOrderNumber()
        {
            var orderNumber =
                await _weavingOrderDocumentRepository.GetWeavingOrderNumber();

            return Ok(orderNumber);
        }

        //[HttpGet("order-by-period/{month}/{year}/unit-name/{unit}/unit-id/{unitId}/status/{status}")]
        //public async Task<IActionResult> Get(string month,
        //                                     string year,
        //                                     string unit,
        //                                     int unitId,
        //                                     string status)
        //{
        //    var acceptRequest = Request.Headers.Values.ToList();
        //    var index = acceptRequest.IndexOf("application/pdf") > 0;
            
        //    var resultData = new List<OrderReportBySearchDto>();
        //    var query =
        //        _weavingOrderDocumentRepository
        //            .Query.OrderByDescending(item => item.CreatedDate);
        //    var orderDto =
        //        _weavingOrderDocumentRepository
        //            .Find(query).Where(entity => entity.Period.Month == (month) &&
        //                                         entity.Period.Year == (year) &&
        //                                         entity.UnitId.Value == (unitId));

        //    if (status.Equals(Constants.ONORDER))
        //    {
        //        orderDto = orderDto.Where(e => e.OrderStatus == Constants.ONORDER).ToList();
        //    }

        //    if (status.Equals(Constants.ALL))
        //    {
        //        orderDto = orderDto.Where(e => e.OrderStatus != "").ToList();
        //    }

        //    foreach (var order in orderDto)
        //    {
        //        var constructionDocument =
        //            _constructionDocumentRepository
        //                .Find(e => e.Identity.Equals(order.ConstructionId.Value))
        //                .FirstOrDefault();

        //        var estimationQuery =
        //        _estimationProductRepository.Query.OrderByDescending(item => item.CreatedDate);
        //        var estimationDocument =
        //            _estimationProductRepository.Find(estimationQuery.Include(p => p.EstimationProducts))
        //                                        .Where(v => v.Period.Month.Equals(order.Period.Month) && v.Period.Year.Equals(order.Period.Year) && v.UnitId.Value.Equals(order.UnitId.Value)).ToList();

        //        var warpMaterials = new List<string>();
        //        foreach (var item in constructionDocument.ListOfWarp)
        //        {
        //            var material = _yarnDocumentRepository.Find(o => o.Identity == item.YarnId.Value).FirstOrDefault();
        //            if (material != null)
        //            {
        //                if (!warpMaterials.Contains(material.Name))
        //                {
        //                    warpMaterials.Add(material.Name);
        //                }
        //            }
        //        }

        //        var weftMaterials = new List<string>();
        //        foreach (var item in constructionDocument.ListOfWarp)
        //        {
        //            var material = _yarnDocumentRepository.Find(o => o.Identity == item.YarnId.Value).FirstOrDefault();
        //            if (material != null)
        //            {
        //                if (!weftMaterials.Contains(material.Name))
        //                {
        //                    weftMaterials.Add(material.Name);
        //                }
        //            }
        //        }

        //        var warpType = "";
        //        foreach (var item in warpMaterials)
        //        {
        //            if (warpType == "")
        //            {
        //                warpType = item;
        //            }
        //            else
        //            {
        //                warpType = warpType + item;
        //            }
        //        }

        //        var weftType = "";
        //        foreach (var item in weftMaterials)
        //        {
        //            if (weftType == "")
        //            {
        //                weftType = item;
        //            }
        //            else
        //            {
        //                weftType = item + item;
        //            }
        //        }

        //        var yarnNumber = warpType + "X" + weftType;

        //        if (constructionDocument == null)
        //        {
        //            throw Validator.ErrorValidation(("Construction Document",
        //                                             "Invalid Construction Document with Order Identity " +
        //                                             order.Identity +
        //                                             " Not Found"));
        //        }

        //        var newOrder = new OrderReportBySearchDto(order, constructionDocument, estimationDocument, yarnNumber, unit);

        //        resultData.Add(newOrder);
        //    }

        //    await Task.Yield();

        //    if (index.Equals(true))
        //    {
        //        OrderProductionPdfTemplate pdfTemplate = new OrderProductionPdfTemplate(resultData);
        //        MemoryStream stream = pdfTemplate.GeneratePdfTemplate();
        //        return new FileStreamResult(stream, "application/pdf")
        //        {
        //            FileDownloadName = string.Format("Laporan Surat Order Produksi.pdf")
        //        };
        //    }
        //    else
        //    {
        //        return Ok(resultData);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            page = page - 1;
            var query =
                _weavingOrderDocumentRepository.Query.OrderByDescending(item => item.CreatedDate);
            var weavingOrderDocuments =
                _weavingOrderDocumentRepository.Find(query);

            var orderDocuments = new List<ListWeavingOrderDocumentDto>();

            foreach (var weavingOrder in weavingOrderDocuments)
            {
                var construction =
                    _constructionDocumentRepository.Find(o => o.Identity == weavingOrder.ConstructionId.Value).FirstOrDefault();

                var orderData =
                    new ListWeavingOrderDocumentDto(weavingOrder,
                                                    new FabricConstructionDocument(construction.Identity, construction.ConstructionNumber));

                orderDocuments.Add(orderData);
            }


            if (!string.IsNullOrEmpty(keyword))
            {
                orderDocuments =
                    orderDocuments
                        .Where(entity => entity.OrderNumber.Contains(keyword,
                                                                     StringComparison.OrdinalIgnoreCase) ||
                                         entity.ConstructionNumber.Contains(keyword,
                                                                            StringComparison.OrdinalIgnoreCase) ||
                                         entity.UnitId.Value.ToString().Contains(keyword,
                                                                          StringComparison.OrdinalIgnoreCase) ||
                                         entity.DateOrdered.LocalDateTime
                                                           .ToString("dd MMMM yyyy")
                                                           .Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop =
                    typeof(ListWeavingOrderDocumentDto).GetProperty(key);

                if (orderDictionary.Values.Contains("asc"))
                {
                    orderDocuments =
                        orderDocuments.OrderBy(x => prop.GetValue(x, null)).ToList();
                }
                else
                {
                    orderDocuments =
                        orderDocuments.OrderByDescending(x => prop.GetValue(x, null)).ToList();
                }
            }

            var ResultOrderDocuments =
                orderDocuments.Skip(page * size).Take(size).ToList();
            int totalRows = orderDocuments.Count();
            int resultCount = ResultOrderDocuments.Count();
            page = page + 1;

            await Task.Yield();

            return Ok(ResultOrderDocuments, info: new
            {
                page,
                size,
                total = totalRows,
                count = resultCount
            });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var orderId = Guid.Parse(Id);
            var order =
                _weavingOrderDocumentRepository.Find(item => item.Identity == orderId)
                                               .FirstOrDefault();
            var construction =
                _constructionDocumentRepository.Find(item => item.Identity == order.ConstructionId.Value)
                                               .FirstOrDefault();
            var orderDto = new WeavingOrderDocumentDto(order,
                                                       new UnitId(order.UnitId.Value),
                                                       new FabricConstructionDocument(construction.Identity,
                                                                                      construction.ConstructionNumber));

            await Task.Yield();

            if (orderId == null)
            {
                return NoContent();
            }
            else
            {
                return Ok(orderDto);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]PlaceOrderCommand command)
        {
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(string Id,
                                             [FromBody]UpdateOrderCommand command)
        {
            if (!Guid.TryParse(Id, out Guid orderId))
            {
                return NotFound();
            }

            command.SetId(orderId);
            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(string Id)
        {
            if (!Guid.TryParse(Id, out Guid orderId))
            {
                return NotFound();
            }

            var command = new RemoveOrderCommand();
            command.SetId(orderId);

            var order = await Mediator.Send(command);

            return Ok(order.Identity);
        }

        //Controller for Order Production Report
        [HttpGet("get-report")]
        public async Task<IActionResult> GetReport(int weavingUnitId = 0,
                                                   DateTimeOffset? dateFrom = null,
                                                   DateTimeOffset? dateTo = null,
                                                   int page = 1,
                                                   int size = 25,
                                                   string order = "{}")
        {
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/pdf") > 0;

            var orderProductionReport = await _orderProductionReportQuery.GetReports(weavingUnitId,
                                                                                     dateFrom,
                                                                                     dateTo,
                                                                                     page,
                                                                                     size,
                                                                                     order);

            await Task.Yield();
            if (index.Equals(true))
            {
                OrderProductionPdfTemplate pdfTemplate = new OrderProductionPdfTemplate(orderProductionReport.Item1.ToList());
                MemoryStream orderPdf = pdfTemplate.GeneratePdfTemplate();
                return new FileStreamResult(orderPdf, "application/pdf")
                {
                    FileDownloadName = string.Format("Laporan Surat Order Produksi.pdf")
                };
            }
            else
            {
                return Ok(orderProductionReport.Item1, info: new
                {
                    count = orderProductionReport.Item2
                });
            }
        }
    }
}
