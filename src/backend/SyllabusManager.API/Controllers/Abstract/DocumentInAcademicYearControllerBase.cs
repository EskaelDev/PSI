using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SyllabusManager.API.Helpers;
using SyllabusManager.Data.Models;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Services.Abstract;

namespace SyllabusManager.API.Controllers.Abstract
{
    public abstract class DocumentInAcademicYearControllerBase<T> : ModelBaseControllerBase<T> where T:DocumentInAcademicYearBase
    {
        private IDocumentInAcademicYearService<T> _service;
        protected readonly UserManager<SyllabusManagerUser> _userManager;
        public DocumentInAcademicYearControllerBase(IDocumentInAcademicYearService<T> service, UserManager<SyllabusManagerUser> userManager) :base(service)
        {
            _userManager = userManager;
            _service = service;
        }

        protected async Task<bool> CheckIfUserIsFosSupervisor(string fosCode)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            return await _service.CheckIfFosSupervisor(user, fosCode);
        }

        protected async Task<bool> CheckIfUserIsFosSupervisor(Guid documentId)
        {
            var user = await AuthenticationHelper.GetAuthorizedUser(HttpContext.User, _userManager);
            return await _service.CheckIfFosSupervisor(user, documentId);
        }
    }
}
