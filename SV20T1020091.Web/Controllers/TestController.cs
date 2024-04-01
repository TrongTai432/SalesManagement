using Microsoft.AspNetCore.Mvc;
using System.Globalization;
namespace SV20T1020091.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Create()
        {
            var model = new Models.Person()
            {
                Name = "Test",
                Birthday = new DateTime(2002, 11, 07),
                Salary = 10.25m
            };
            return View(model);
        }

        public IActionResult Save(Models.Person model, string birthDayInput = "")
        {
            DateTime? d = null;
            try
            {
                d = DateTime.ParseExact(birthDayInput, "d/M/yyyy" ,CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                
            }
            if (d.HasValue)
            {
                model.Birthday = d.Value;
            }
            return Json(model);
        }
    }
}
