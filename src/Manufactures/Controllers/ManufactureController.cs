using ExtCore.Data.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Manufactures.ViewModels.Weaving;

namespace Manufactures.Controllers
{
    public class ManufactureController : Barebone.Controllers.ControllerBase
    {
        public ManufactureController(IStorage storage) : base(storage)
        {
        }

        public ActionResult Index()
        {
            return this.View(new IndexViewModelFactory().Create(this.Storage));
        }
    }
}
