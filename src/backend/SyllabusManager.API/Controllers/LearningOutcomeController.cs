﻿using Microsoft.AspNetCore.Mvc;
using SyllabusManager.API.Controllers.Abstract;
using SyllabusManager.Data.Models.LearningOutcomes;
using SyllabusManager.Logic.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearningOutcomeController : DocumentInAcademicYearControllerBase<LearningOutcomeDocument>
    {
        private readonly LearningOutcomeService _learningOutcomeService;

        public LearningOutcomeController(LearningOutcomeService learningOutcomeService) : base(learningOutcomeService)
        {
            _learningOutcomeService = learningOutcomeService;
        }
        /// <summary>
        /// Zwraca obiekt LearningOutcomeDocument o najnowszej wersji dla podanych parametrów (jeżeli nie istnieje to zwraca nowy obiekt)
        /// </summary>
        /// <param name="fosCode">Kod FieldOfStudy(Kierunku studiów)</param>
        /// <param name="academicYear">Rok akademicki</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Latest([FromQuery(Name = "fos")] string fosCode,
                                                [FromQuery(Name = "year")] string academicYear)
        {
            LearningOutcomeDocument result = await _learningOutcomeService.Latest(fosCode, academicYear);

            if (result is null)
                return NotFound();

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
            LearningOutcomeDocument result = await _learningOutcomeService.Save(learningOutcome);

            if (result is null)
                return BadRequest();
            return Ok(result);
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
            LearningOutcomeDocument result = await _learningOutcomeService.SaveAs(fosCode, academicYear, learningOutcome);
            if (result is null)
                return BadRequest();
            return Ok(result);
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
        public async Task<IActionResult> ImportFrom(string currentDocId,
                                                   [FromQuery(Name = "fos")] string fosCode,
                                                   [FromQuery(Name = "year")] string academicYear)
        {
            LearningOutcomeDocument result = await _learningOutcomeService.ImportFrom(currentDocId, fosCode, academicYear);
            if (result is null)
                return NotFound();
            return Ok(result);
        }
        // todo: /delete/{currentDocId} -> usuwa, ale jako IsDeleted (wszystkie wersje)
        [HttpDelete]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Delete(string currentDocId)
        {
            bool result = await _learningOutcomeService.Delete(currentDocId);
            if (result)
                return Ok();
            return NotFound();

        }
        // todo: /pdf/{currentDocId} -> generuje pdf

        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(string currentDocId)
        {
            return Ok();
        }
        // todo: /pdf/{currentDocId}?version={version} -> generuje pdf z wersji
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> Pdf(string currentDocId,
                                            [FromQuery(Name = "version")] string version)
        {
            return Ok();
        }
        // todo: /history/{currentDocId} -> pobiera historię wersji (jako lista string z nazwami wersji)
        [HttpGet]
        [Route("{currentDocId}")]
        public async Task<IActionResult> History(string currentDocId)
        {
            List<string> result = await _learningOutcomeService.History(currentDocId);
            if (result is null)
                return NotFound();
            return Ok();
        }
    }
}
