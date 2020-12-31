using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models.DTO;
using SyllabusManager.Logic.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyllabusManager.Logic.Services
{
    public class FieldOfStudyService : NonVersionedService<FieldOfStudy>, IFieldOfStudyService
    {
        private readonly IUserService _userService;

        public FieldOfStudyService(SyllabusManagerDbContext dbContext, IUserService userService) : base(dbContext)
        {
            _userService = userService;
        }

        public async Task<List<UserDTO>> GetPossibleSupervisors()
        {
            return await _userService.GetTeachers();
        }

        public override async Task<List<FieldOfStudy>> GetAllAsync()
        {
            return await _dbSet.Where(e => !e.IsDeleted).Include(fos => fos.Specializations).Where(fos => fos.Specializations.Any(s => !s.IsDeleted)).AsNoTracking().ToListAsync();
        }

        public override async Task<FieldOfStudy> SaveAsync(FieldOfStudy entity)
        {

            FieldOfStudy entityDb = _dbSet.Find(entity.Id);
            if (entityDb == null)
            {
                await _dbSet.AddAsync(entity);
            }
            else
            {
                await UpdateSpecializationsAsync(entity, entityDb);
                _dbSet.Update(entity);
            }
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        private async Task UpdateSpecializationsAsync(FieldOfStudy newFos, FieldOfStudy oldFos)
        {
            _dbContext.Specializations.RemoveRange(oldFos.Specializations);
            await _dbContext.Specializations.AddRangeAsync(newFos.Specializations);
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<bool> DeleteAsync(string id)
        {
            FieldOfStudy fos = await GetByIdAsync(id);
            _dbContext.Specializations.RemoveRange(fos.Specializations);
            EntityEntry<FieldOfStudy> result = _dbSet.Remove(fos);

            await _dbContext.SaveChangesAsync();

            if (result.State == EntityState.Deleted)
                return true;

            return false;

        }

        public override async Task<bool> SoftDeleteAsync(string id)
        {
            FieldOfStudy fos = await GetByIdAsync(id);
            if (fos == null)
                return false;

            fos.Specializations.ForEach(s => s.IsDeleted = true);
            fos.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public override async Task<FieldOfStudy> GetByIdAsync(string id)
        {
            return await _dbSet.Where(e => e.Id.ToString() == id).Include(e => e.Specializations).FirstOrDefaultAsync();
        }
    }

}
