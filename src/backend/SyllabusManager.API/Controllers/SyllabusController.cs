using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models.Syllabuses;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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

            Syllabus result = await _syllabusService.Latest(fos, spec, year);

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

            SyllabusManagerUser user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.Save(syllabus, user);

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

            SyllabusManagerUser user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.SaveAs(fos, spec, year, syllabus, user);

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

            SyllabusManagerUser user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            Syllabus result = await _syllabusService.ImportFrom(currentDocId, fos, spec, year, user);

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


        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId)
        {
            bool result = await _syllabusService.Pdf(currentDocId);
            if (result == false)
            {
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(PdfHelper.PATH_PAGED, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", true);

        }

        [HttpGet]
        public async Task<IActionResult> Pdf([FromQuery] string fos,
                                             [FromQuery] string spec,
                                             [FromQuery] string year)
        {
            bool result = await _syllabusService.Pdf(fos, spec, year);
            if (result == false)
            {
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(PdfHelper.PATH_PAGED, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, "application/pdf", true);

        }


        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> PlanPdf(Guid currentDocId)
        {
            bool result = await _syllabusService.PlanPdf(currentDocId);
            if (result == false)
            {
                return NotFound();
            }

            MemoryStream memory = new MemoryStream();
            using (FileStream stream = new FileStream(PdfHelper.PATH_PAGED, FileMode.Open))
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
