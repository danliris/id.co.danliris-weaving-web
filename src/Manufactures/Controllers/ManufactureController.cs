using ExtCore.Data.Abstractions;
using Manufactures.Domain.Orders;
using Manufactures.Domain.Orders.Repositories;
using Manufactures.ViewModels.Manufacture;
using Microsoft.AspNetCore.Mvc;
using Moonlay.ExtCore.Mvc.Abstractions;

namespace Manufactures.Controllers
{
    public class ManufactureController : Barebone.Controllers.ControllerBase
    {
        private readonly IWorkContext _workContext;

        public ManufactureController(IWorkContext workContext, IStorage storage) : base(storage)
        {
            _workContext = workContext;
        }

        public ActionResult Index()
        {
            return this.View(new IndexViewModelFactory().Create(this.Storage));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel createViewModel)
        {
            if (this.ModelState.IsValid)
            {
                ManufactureOrder order = new CreateViewModelMapper().Map(createViewModel, _workContext.CurrentUser);

                this.Storage.GetRepository<IManufactureOrderRepository>().Update(order).Wait();

                this.Storage.Save();
                return this.RedirectToAction("index");
            }

            return this.View();
        }
    }
}