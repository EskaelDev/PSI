using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers.Abstract
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public abstract class ApiController : ControllerBase
    {
    }
}
