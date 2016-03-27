using System;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    public class TestErrorController : Controller
    {
        public ActionResult Index()
        {
            throw new ApplicationException("Test error");
        }
    }
}