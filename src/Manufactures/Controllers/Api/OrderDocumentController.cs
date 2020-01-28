using Barebone.Controllers;
using Manufactures.Domain.FabricConstructions.Repositories;
using Manufactures.Domain.Orders.Commands;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.Domain.Shared.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manufactures.Domain.Estimations.Productions.Repositories;
using Manufactures.Domain.Yarns.Repositories;
using Manufactures.Helpers.PdfTemplates;
using Manufactures.Application.Orders.DataTransferObjects.OrderReport;
using Manufactures.Domain.Orders.Queries.OrderReport;
using Manufactures.Application.Orders.DataTransferObjects;
using Manufactures.Domain.Orders.Queries;

namespace Manufactures.Controllers.Api
{
    [Produces("application/json")]
    [Route("weaving/orders")]
    [ApiController]
    [Authorize]
    public class OrderDocumentController : ControllerApiBase
    {
        private readonly IOrderRepository 
            _weavingOrderDocumentRepository;
        private readonly IFabricConstructionRepository 
            _constructionDocumentRepository;
        private readonly IEstimatedProductionDocumentRepository 
            _estimationProductRepository;
        private readonly IYarnDocumentRepository 
            _yarnDocumentRepository;

        private readonly IOrderQuery<OrderListDto> _orderQuery;
        private readonly IOrderReportQuery<OrderReportListDto> _orderProductionReportQuery;

        public OrderDocumentController(IServiceProvider serviceProvider,
                                       IWorkContext workContext,
                                       IOrderQuery<OrderListDto> orderQuery,
                                       IOrderReportQuery<OrderReportListDto> orderProductionReportQuery) : base(serviceProvider)
        {
            _weavingOrderDocumentRepository =
                this.Storage.GetRepository<IOrderRepository>();
            _constructionDocumentRepository =
                this.Storage.GetRepository<IFabricConstructionRepository>();
            _estimationProductRepository =
                this.Storage.GetRepository<IEstimatedProductionDocumentRepository>();
            _yarnDocumentRepository =
                this.Storage.GetRepository<IYarnDocumentRepository>();

            _orderQuery = orderQuery ?? throw new ArgumentNullException(nameof(orderQuery));
            _orderProductionReportQuery = orderProductionReportQuery ?? throw new ArgumentNullException(nameof(orderProductionReportQuery));
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page = 1,
                                             int size = 25,
                                             string order = "{}",
                                             string keyword = null,
                                             string filter = "{}")
        {
            var orderDocuments = await _orderQuery.GetAll();


            if (!string.IsNullOrEmpty(keyword))
            {
                orderDocuments =
                    orderDocuments
                        .Where(o => o.OrderNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    o.ConstructionNumber.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    o.Unit.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    o.Period.Month.ToString("MMMM").Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                    o.Period.Year.ToString("yyyy").Contains(keyword, StringComparison.OrdinalIgnoreCase))
                        .ToList();
            }

            if (!order.Contains("{}"))
            {
                Dictionary<string, string> orderDictionary =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
                var key = orderDictionary.Keys.First().Substring(0, 1).ToUpper() +
                          orderDictionary.Keys.First().Substring(1);
                System.Reflection.PropertyInfo prop = typeof(OrderListDto).GetProperty(key);

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

            var result = orderDocuments.Skip((page - 1) * size).Take(size);
            var total = result.Count();

            return Ok(result, info: new { page, size, total });
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(string Id)
        {
            var identity = Guid.Parse(Id);
            var orderDocument = await _orderQuery.GetById(identity);

            if (orderDocument == null)
            {
                return NotFound(identity);
            }

            return Ok(orderDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddOrderCommand command)
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
        public async Task<IActionResult> GetReport(int unitId = 0,
                                                   DateTimeOffset? dateFrom = null,
                                                   DateTimeOffset? dateTo = null,
                                                   int page = 1,
                                                   int size = 25,
                                                   string order = "{}")
        {
            var acceptRequest = Request.Headers.Values.ToList();
            var index = acceptRequest.IndexOf("application/pdf") > 0;

            var orderProductionReport = await _orderProductionReportQuery.GetReports(unitId,
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
