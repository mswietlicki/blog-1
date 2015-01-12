using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sychev.Monitoring.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chart([Required]Guid id)
        {
            return View();
        }
	}
}