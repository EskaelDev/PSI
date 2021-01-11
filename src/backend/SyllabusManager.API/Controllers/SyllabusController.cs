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
using System.IO;
using SyllabusManager.Logic.Helpers;

namespace SyllabusManager.API.Controllers
{
    public class SyllabusController : DocumentInAcademicYearControllerBase<Syllabus>
    {
        private readonly ISyllabusService _syllabusService;

        public SyllabusController(ISyllabusService syllabusService, UserManager<SyllabusManagerUser> userManager) : base(syllabusService, userManager)
        {
            _syllabusService = syllabusService;
        }

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
            if (!await CheckIfUserIsFosSupervisor(fos)) return Forbid();

            var result = await _syllabusService.Latest(fos, spec, year);
            
            if (result is null) return NotFound();
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
            if (!await CheckIfUserIsFosSupervisor(syllabus.FieldOfStudy.Code)) return Forbid();

            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var result = await _syllabusService.Save(syllabus, user);
            
            if (result is null) return BadRequest();
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
            if (!await CheckIfUserIsFosSupervisor(fos)) return Forbid();

            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var result = await _syllabusService.SaveAs(fos, spec, year, syllabus, user);
            
            if (result is null) return BadRequest();
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
            if (!await CheckIfUserIsFosSupervisor(currentDocId)) return Forbid();

            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            var result = await _syllabusService.ImportFrom(currentDocId, fos, spec, year, user);
            
            if (result is null) return NotFound();
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
            if (!await CheckIfUserIsFosSupervisor(currentDocId)) return Forbid();

            bool result = await _syllabusService.Delete(currentDocId);
            
            if (result) return Ok();
            return NotFound();
        }

        // todo: /pdf/{currentDocId}?version={version} -> generuje pdf z wersji
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId,
                                            [FromQuery] string version)
        {
            var result = await _syllabusService.Pdf(currentDocId);
            if (result == false)
            {
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
