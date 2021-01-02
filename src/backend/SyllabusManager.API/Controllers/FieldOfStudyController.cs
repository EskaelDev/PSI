using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Services;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    [Authorize(Roles = UsersRoles.AdminTeacher)]
    [ApiController]
    public class FieldOfStudyController : NonVersionedControllerBase<FieldOfStudy>
    {
        private readonly IFieldOfStudyService _fieldOfStudyService;

        public FieldOfStudyController(IFieldOfStudyService fieldOfStudyService) : base(fieldOfStudyService)
        {
            _fieldOfStudyService = fieldOfStudyService;
        }

        [HttpGet]
        public async Task<IActionResult> PossibleSupervisors()
        {
            return Ok(await _fieldOfStudyService.GetPossibleSupervisors());
        }

        [HttpGet]
        public override async Task<IActionResult> All()
        {
            return Ok(await _fieldOfStudyService.GetAllAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] FieldOfStudy fos)
        {
            FieldOfStudy result = await _fieldOfStudyService.SaveAsync(fos);
            if (result is null)
                return BadRequest();
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            bool result = await _fieldOfStudyService.SoftDeleteAsync(id);
            if (result)
                return Ok();
            return NotFound();
        }
    }
}
