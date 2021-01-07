using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Models;
using SyllabusManager.Logic.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.API.Controllers
{
    [Authorize(Roles = UsersRoles.AdminTeacher)]
    public class FieldOfStudyController : NonVersionedControllerBase<FieldOfStudy>
    {
        private readonly IFieldOfStudyService _fieldOfStudyService;
        private readonly UserManager<SyllabusManagerUser> _userManager;

        public FieldOfStudyController(IFieldOfStudyService fieldOfStudyService, UserManager<SyllabusManagerUser> userManager) : base(fieldOfStudyService)
        {
            _fieldOfStudyService = fieldOfStudyService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> AllMy()
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            if (await AuthenticationHelper.CheckIfAdmin(user, _userManager)) return Ok(await _modelService.GetAllAsync());
            return Ok(await _fieldOfStudyService.GetAllMy(user));
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
