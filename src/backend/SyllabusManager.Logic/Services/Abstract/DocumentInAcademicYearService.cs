using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services.Abstract
{
    public abstract class DocumentInAcademicYearService<T> : ModelBaseService<T>, IDocumentInAcademicYearService<T> where T : DocumentInAcademicYearBase
    {
        private readonly UserManager<SyllabusManagerUser> _userManager;

        protected DocumentInAcademicYearService(SyllabusManagerDbContext dbContext, UserManager<SyllabusManagerUser> userManager) : base(dbContext)
        {
            _userManager = userManager;
        }

        public async Task<bool> CheckIfFosSupervisor(SyllabusManagerUser user, Guid documentId)
        {
            var fosCode = GetFosCode(documentId);
            return !(fosCode is null) && await CheckIfFosSupervisor(user, fosCode);
        }

        public async Task<bool> CheckIfFosSupervisor(SyllabusManagerUser user, string fosCode)
        {
            return await AuthorizationHelper.CheckIfAdmin(user, _userManager) 
                   || _dbContext.FieldsOfStudies.Include(f => f.Supervisor)
                       .FirstOrDefault(f => f.Code == fosCode)?.Supervisor.Id == user.Id;
        }

        protected string IncreaseVersion(string version)
        {
            string newVersion = DateTime.UtcNow.ToString("yyyyMMdd");

            if (version.Substring(0, 8) == newVersion)
            {
                string currentV = version.Substring(8);
                int newV = int.Parse(currentV) + 1;
                return version.Substring(0, 8) + newV.ToString("00");
            }

            return newVersion + "01";
        }

        private string GetFosCode(Guid documentId)
        {
            return _dbSet.Include(l => l.FieldOfStudy)
                .FirstOrDefault(l => l.Id == documentId)?.FieldOfStudy?.Code;
        }
    }
}
