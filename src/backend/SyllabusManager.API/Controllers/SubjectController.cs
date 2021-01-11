using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models.Subjects;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using SyllabusManager.Logic.Services;
using System;
using System.IO;
using System.Threading.Tasks;

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
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var result = await _subjectService.GetAll(fos, spec, year, user);
            foreach (var r in result)
            {
                r.IsAdmin = await CheckIfUserIsFosSupervisor(r.FieldOfStudy.Code);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Latest(
                                                [FromQuery] string fos,
                                                [FromQuery] string spec,
                                                [FromQuery] string code,
                                                [FromQuery] string year)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var result = await _subjectService.Latest(fos, spec, code, year, user);
            
            if (result is null) return NotFound();

            result.IsAdmin = await CheckIfUserIsFosSupervisor(result.FieldOfStudy.Code);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Subject subject)
        {
            if (subject.Id == Guid.Empty)
            {
                if (!await CheckIfUserIsFosSupervisor(subject.FieldOfStudy.Code)) return Forbid();
            }
            else
            {
                var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
                var supervisor = _subjectService.GetSupervisorId(subject.Id);
                if (!await CheckIfUserIsFosSupervisor(subject.FieldOfStudy.Code) && supervisor != user.Id) return Forbid();
            }
            
            var result = await _subjectService.Save(subject);
            return result switch
            {
                0 => Ok(),
                1 => NotFound(),
                2 => Conflict(),
                _ => BadRequest()
            };
        }

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> ImportFrom(Guid currentDocId,
                                                   [FromQuery] string fos,
                                                   [FromQuery] string spec,
                                                   [FromQuery] string code,
                                                   [FromQuery] string year)
        {

            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var supervisor = _subjectService.GetSupervisorId(currentDocId);
            if (!await CheckIfUserIsFosSupervisor(currentDocId) && supervisor != user.Id) return Forbid();

            var result = await _subjectService.ImportFrom(currentDocId, fos, spec, code, year);

            return result switch
            {
                0 => Ok(),
                1 => NotFound(),
                2 => Conflict(),
                _ => BadRequest()
            };
        }

        [HttpDelete]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Delete(Guid currentDocId)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var supervisor = _subjectService.GetSupervisorId(currentDocId);
            if (!await CheckIfUserIsFosSupervisor(currentDocId) && supervisor != user.Id) return Forbid();
            
            bool result = await _subjectService.Delete(currentDocId);
            
            if (result) return Ok();
            return NotFound();
        }

        // todo: /pdf/{currentDocId}?version={version} -> generuje pdf z wersji
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId)
        {
            Log.Information("Generating pdf for Subject id:" + currentDocId.ToString());

            var result = await _subjectService.Pdf(currentDocId);
            if (result == false)
            {
                Log.Error("Subject id " + currentDocId.ToString() + " not found");
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(PdfHelper.PATH, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", true);
        }

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> History(Guid currentDocId)
        {
            var result = await _subjectService.History(currentDocId);
            
            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
