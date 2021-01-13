using Microsoft.AspNetCore.Mvc;
using SyllabusManager.Logic.Helpers;
using SyllabusManager.Logic.Services;

namespace SyllabusManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Route("IsAlive")]
        public IActionResult IsAlive() => Ok("SyllabusManager is running");
    }
}
