using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {


        public void Test()
        {
            new SubjectService().SubjectTest();
        }
    }
}
