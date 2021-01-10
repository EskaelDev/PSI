using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Models.DTO;
using SyllabusManager.Logic.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;
using SyllabusManager.Data.Models.User;

namespace SyllabusManager.Logic.Services
{
    public interface IFieldOfStudyService : INonVersionedService<FieldOfStudy>
    {
        Task<List<UserDTO>> GetPossibleSupervisors();
        Task<List<FieldOfStudy>> GetAllMy(SyllabusManagerUser user);
    }
}