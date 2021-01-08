using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.API.Controllers
{
    public class SyllabusController : DocumentInAcademicYearControllerBase<Syllabus>
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusController(ISyllabusService syllabusService, UserManager<SyllabusManagerUser> userManager) : base(syllabusService, userManager)
        {
            _syllabusService = syllabusService;
        }

        // todo: /latest?fos={fosCode}&spec={specCode}&year={academicYear} -> zwraca obiekt Syllabus o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fos">Field of Study code</param>
        /// <param name="spec">Specialization code</param>
        /// <param name="year">Academic year</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Latest(
                                                [FromQuery] string fos,
                                                [FromQuery] string spec,
                                                [FromQuery] string year)
        {
            Syllabus result = await _syllabusService.Latest(fos, spec, year);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Zapisuje obiekt w najnowsze wersji
        /// </summary>
        /// <param name="syllabus"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save([FromBody] Syllabus syllabus)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.Save(syllabus, user);
            if (result is null)
                return BadRequest();

            return Ok(result);
        }

        /// <summary>
        /// Zapisuje obiekt w najnowszej wersji ale jako inny obiekt o podanych parametrach
        /// </summary>
        /// <param name="fos"></param>
        /// <param name="spec"></param>
        /// <param name="year"></param>
        /// <param name="syllabus"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<IActionResult> SaveAs([FromQuery] string fos,
                                                [FromQuery] string spec,
                                                [FromQuery] string year,
                                                [FromBody] Syllabus syllabus)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.SaveAs(fos, spec, year, syllabus, user);
            if (result is null)
                return BadRequest();
            return Ok();
        }


        /// <summary>
        /// Pobiera najnowszą wersję z obiektu o podanych parametrach i zapisuje jej kopię jako najnowsza wersja obiektu
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <param name="fos"></param>
        /// <param name="spec"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> ImportFrom(Guid currentDocId,
                                                   [FromQuery] string fos,
                                                   [FromQuery] string spec,
                                                   [FromQuery] string year)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.ImportFrom(currentDocId, fos, spec, year, user);
            if (result is null)
                return NotFound();
            return Ok();
        }

        /// <summary>
        /// Usuwa, ale jako IsDeleted (wszystkie wersje)
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Delete(Guid currentDocId)
        {
            bool result = await _syllabusService.Delete(currentDocId);
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


        /// <summary>
        /// Pobiera historię wersji (jako lista string z nazwami wersji)
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> History(Guid currentDocId)
        {
            List<string> result = await _syllabusService.History(currentDocId);
            if (result is null)
                return NotFound();
            return Ok(result);
        }
    }
}
