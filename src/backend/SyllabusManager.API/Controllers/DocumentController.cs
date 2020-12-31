using Microsoft.AspNetCore.Mvc;

namespace SyllabusManager.API.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
