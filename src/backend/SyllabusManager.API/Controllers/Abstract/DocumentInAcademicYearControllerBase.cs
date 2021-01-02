using SyllabusManager.Data.Models;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.API.Controllers.Abstract
{
    public abstract class DocumentInAcademicYearControllerBase<T> : ModelBaseControllerBase<T> where T:DocumentInAcademicYearBase
    {
        public DocumentInAcademicYearControllerBase(DocumentInAcademicYearService<T> service):base(service)
        {

        }
    }
}
