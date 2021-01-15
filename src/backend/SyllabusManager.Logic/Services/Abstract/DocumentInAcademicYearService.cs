using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SyllabusManager.Data;
using SyllabusManager.Data.Models;
using SyllabusManager.Data.Models.User;
using SyllabusManager.Logic.Helpers;
using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public static string IncreaseVersion(string version)
        {
            if (!Regex.IsMatch(version, @"^\d{4}_\d{2}_\d{2}_\d{2}$")) return version;

            string newVersion = DateTime.UtcNow.ToString("yyyy_MM_dd");

            if (version.Substring(0, 10) == newVersion)
            {
                string currentV = version[11..];
                int newV = int.Parse(currentV) + 1;
                return version.Substring(0, 10) + "_" + newV.ToString("00");
            }

            return newVersion + "_01";
        }

        public static string NewVersion() => DateTime.UtcNow.ToString("yyyy_MM_dd_") + "01";

        private string GetFosCode(Guid documentId)
        {
            return _dbSet.Include(l => l.FieldOfStudy)
                .FirstOrDefault(l => l.Id == documentId)?.FieldOfStudy?.Code;
        }
    }
}
