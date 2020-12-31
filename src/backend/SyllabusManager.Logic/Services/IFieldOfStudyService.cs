using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Models.DTO;
using SyllabusManager.Logic.Services.Abstract;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public interface IFieldOfStudyService : INonVersionedService<FieldOfStudy>
    {
        Task<List<UserDTO>> GetPossibleSupervisors();
    }
}