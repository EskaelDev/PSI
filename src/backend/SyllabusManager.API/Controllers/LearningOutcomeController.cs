using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using SyllabusManager.Logic.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    public class LearningOutcomeController : DocumentInAcademicYearControllerBase<LearningOutcomeDocument>
    {
        private readonly ILearningOutcomeService _learningOutcomeService;

        public LearningOutcomeController(ILearningOutcomeService learningOutcomeService, UserManager<SyllabusManagerUser> userManager) : base(learningOutcomeService, userManager)
        {
            _learningOutcomeService = learningOutcomeService;
        }

        /// <summary>
        /// Zwraca obiekt LearningOutcomeDocument o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// </summary>
        /// <param name="fosCode">Kod FieldOfStudy(Kierunku studiów)</param>
        /// <param name="academicYear">Rok akademicki</param>
        /// <param name="readOnly">Tylko do odczytu</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Latest([FromQuery(Name = "fos")] string fosCode,
                                                [FromQuery(Name = "year")] string academicYear,
                                                [FromQuery(Name = "readOnly")] bool readOnly)
        {
            if (!readOnly && !await CheckIfUserIsFosSupervisor(fosCode)) return Forbid();

            LearningOutcomeDocument result = await _learningOutcomeService.Latest(fosCode, academicYear, readOnly);

            if (result is null) return NotFound();

            return Ok(result);
        }

        /// <summary>
        /// Zapisuje obiekt w najnowszej wersji
        /// </summary>
        /// <param name="learningOutcome"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Save(LearningOutcomeDocument learningOutcome)
        {
            if (!await CheckIfUserIsFosSupervisor(learningOutcome.FieldOfStudy.Code)) return Forbid();

            LearningOutcomeDocument result = await _learningOutcomeService.Save(learningOutcome);

            if (result is null) return BadRequest();
            return Ok();
        }

        /// <summary>
        /// Zapisuje obiekt w najnowszej wersji ale jako inny obiekt o podanych parametrach
        /// </summary>
        /// <param name="fosCode"></param>
        /// <param name="academicYear"></param>
        /// <param name="learningOutcome"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SaveAs([FromQuery(Name = "fos")] string fosCode,
                                                [FromQuery(Name = "year")] string academicYear,
                                                [FromBody] LearningOutcomeDocument learningOutcome)
        {
            if (!await CheckIfUserIsFosSupervisor(fosCode)) return Forbid();

            LearningOutcomeDocument result = await _learningOutcomeService.SaveAs(fosCode, academicYear, learningOutcome);

            if (result is null) return BadRequest();
            return Ok();
        }

        /// <summary>
        /// Pobiera najnowszą wersję z obiektu o podanych parametrach i zapisuje jej kopię jako najnowsza wersja obiektu
        /// </summary>
        /// <param name="currentDocId"></param>
        /// <param name="fosCode"></param>
        /// <param name="academicYear"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> ImportFrom(Guid currentDocId,
                                                   [FromQuery(Name = "fos")] string fosCode,
                                                   [FromQuery(Name = "year")] string academicYear)
        {
            if (!await CheckIfUserIsFosSupervisor(currentDocId)) return Forbid();

            LearningOutcomeDocument result = await _learningOutcomeService.ImportFrom(currentDocId, fosCode, academicYear);

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

            bool result = await _learningOutcomeService.Delete(currentDocId);

            if (result) return Ok();
            return NotFound();
        }

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(Guid currentDocId)
        {

            bool result = await _learningOutcomeService.Pdf(currentDocId);
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
        public async Task<IActionResult> Pdf([FromQuery(Name = "fos")] string fosCode,
                                             [FromQuery(Name = "year")] string academicYear)
        {
            bool result = await _learningOutcomeService.Pdf(fosCode, academicYear);
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
            System.Collections.Generic.List<string> result = await _learningOutcomeService.History(currentDocId);

            if (result is null) return NotFound();
            return Ok(result);
        }
    }
}
