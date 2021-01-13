using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SyllabusManager.Data.Models;
using SyllabusManager.Logic.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers.Abstract
{
    [Authorize]
    public abstract class NonVersionedControllerBase<T> : ApiControllerBase where T : NonVersionedModelBase
    {
        protected readonly INonVersionedService<T> _modelService;

        protected NonVersionedControllerBase(INonVersionedService<T> crudService)
        {
            _modelService = crudService;
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<IActionResult> ById(string id)
        {
            T result = await _modelService.GetByCodeAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpGet]
        public virtual async Task<IActionResult> All()
        {
            List<T> result = await _modelService.GetAllAsync();
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
