using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyllabusManager.Logic.Helpers;

namespace SyllabusManager.API.Controllers
{
    public class SubjectController : DocumentInAcademicYearControllerBase<Subject>
    {
        private readonly ISubjectService _subjectService;
        private readonly IFieldOfStudyService _fosService;

        public SubjectController(ISubjectService subjectService, IFieldOfStudyService fosService, UserManager<SyllabusManagerUser> userManager) : base(subjectService, userManager)
        {
            _subjectService = subjectService;
            _fosService = fosService;
        }

        [HttpGet]
        public async Task<IActionResult> PossibleTeachers()
        {
            return Ok(await _fosService.GetPossibleSupervisors());
        }

        [HttpGet]
        public async Task<IActionResult> All([FromQuery] string fos,
            [FromQuery] string spec,
            [FromQuery] string year)
        {
            return Ok(await _subjectService.GetAll(fos, spec, year));
        }

        [HttpGet]
        public async Task<IActionResult> AllEditable([FromQuery] string fos,
            [FromQuery] string spec,
            [FromQuery] string year,
            [FromQuery] bool onlyMy)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            if (await AuthorizationHelper.CheckIfAdmin(user, _userManager)) return Ok(await _subjectService.GetAll(fos, spec, year));
            return Ok(await _subjectService.GetAllForUser(fos, spec, year, user, onlyMy));
        }

        [HttpGet]
        public async Task<IActionResult> Latest(
                                                [FromQuery] string fos,
                                                [FromQuery] string spec,
                                                [FromQuery] string code,
                                                [FromQuery] string year)
        {
            var result = await _subjectService.Latest(fos, spec, code, year);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Subject subject)
        {
            var result = await _subjectService.Save(subject);
            if (result is null)
                return Conflict();

            return Ok(result);
        }

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> ImportFrom(Guid currentDocId,
                                                   [FromQuery] string fos,
                                                   [FromQuery] string spec,
                                                   [FromQuery] string code,
                                                   [FromQuery] string year)
        {
            var result = await _subjectService.ImportFrom(currentDocId, fos, spec, code, year);
            if (result is null)
                return NotFound();
            return Ok();
        }

        [HttpDelete]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Delete(Guid currentDocId)
        {
            bool result = await _subjectService.Delete(currentDocId);
            if (result)
                return Ok();
            return NotFound();

        }

        // todo: / pdf /{currentDocId} -> generuje pdf
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId)
        {
            return Ok("Not implemented");
        }

        // todo: /pdf/{currentDocId}?version={version} -> generuje pdf z wersji
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId,
                                            [FromQuery] string version)
        {
            return Ok("Not implemented");
        }

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> History(Guid currentDocId)
        {
            List<string> result = await _subjectService.History(currentDocId);
            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
