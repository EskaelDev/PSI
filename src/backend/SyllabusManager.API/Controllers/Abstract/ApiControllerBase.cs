using Microsoft.AspNetCore.Mvc;

namespace SyllabusManager.API.Controllers.Abstract
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public abstract class ApiControllerBase : ControllerBase
    {
    }
}
