using Microsoft.AspNetCore.Mvc;
using SyllabusManager.Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers.Abstract
{
    public abstract class AbstractCrudController<T> : AbstractApiController where T : class
    {
        private readonly AbstractCrudService<T> _crudService;

        public AbstractCrudController(AbstractCrudService<T> crudService)
        {
            _crudService = crudService;
        }
        [HttpPost]
        public async virtual Task Add([FromBody] T entity)
        {
            await _crudService.AddAsync(entity);
        }

        [HttpGet]
        [Route("/{id}")]
        public async virtual Task GetById(string id)
        {

        }

    }
}
