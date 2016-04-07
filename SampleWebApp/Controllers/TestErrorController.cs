using System;
using System.Web.Mvc;

namespace SampleWebApp.Controllers
{
    public class TestErrorController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                int remainder;
                int quotient = Math.DivRem(1, 0, out remainder);
                return Content("Should have thrown an exception by here");
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Test error", ex);
            }
        }
    }
}