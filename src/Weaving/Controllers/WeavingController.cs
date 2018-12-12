using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Weaving.ViewModels.Weaving;

namespace Weaving.Controllers
{
    public class WeavingController : Barebone.Controllers.ControllerBase
    {
        public WeavingController(IStorage storage) : base(storage)
        {
        }

        public ActionResult Index()
        {
            return this.View(new IndexViewModelFactory().Create(this.Storage));
        }
    }
}
