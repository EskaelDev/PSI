using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SyllabusManager.Data;
using SyllabusManager.Data.Models.FieldOfStudies;
using SyllabusManager.Logic.Interfaces;
using SyllabusManager.Logic.Models.DTO;
using SyllabusManager.Logic.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SyllabusManager.Data.Models.User;

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

        public async Task<List<FieldOfStudy>> GetAllMy(SyllabusManagerUser user)
        {
            var all = await _dbSet.Where(e => !e.IsDeleted)
                .Include(fos => fos.Specializations)
                .Include(fos => fos.Supervisor)
                .Where(f => f.Supervisor.Id == user.Id)
                .AsNoTracking()
                .ToListAsync();

            return all.Select(f =>
                {
                    f.Specializations = f.Specializations.Where(s => !s.IsDeleted).ToList();
                    return f;
                })
                .ToList();
        }

        public override async Task<List<FieldOfStudy>> GetAllAsync()
        {
            var all = await _dbSet.Where(e => !e.IsDeleted)
                .Include(fos => fos.Specializations)
                .Include(fos => fos.Supervisor)
                .AsNoTracking()
                .ToListAsync();

            return all.Select(f =>
            {
                f.Specializations = f.Specializations.Where(s => !s.IsDeleted).ToList();
                return f;
            })
            .ToList();
        }

        public override async Task<FieldOfStudy> SaveAsync(FieldOfStudy entity)
        {
            FieldOfStudy entityDb =
                _dbContext.FieldsOfStudies.Include(f => f.Specializations).FirstOrDefault(f => f.Code == entity.Code);
            if (entityDb == null)
            {
                entity.Supervisor = _dbContext.Users.FirstOrDefault(u => u.Id == entity.Supervisor.Id);
                _dbContext.FieldsOfStudies.Add(entity);
            }
            else
            {
                _dbContext.Entry(entityDb).CurrentValues.SetValues(entity);
                UpdateSpecializationsAsync(entity, entityDb);
                entity.Supervisor = _dbContext.Users.FirstOrDefault(u => u.Id == entity.Supervisor.Id);
            }
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        private void UpdateSpecializationsAsync(FieldOfStudy newFos, FieldOfStudy oldFos)
        {
            // delete
            foreach (var spec in oldFos.Specializations)
            {
                if (newFos.Specializations.All(s => s.Code != spec.Code))
                {
                    spec.IsDeleted = true;
                }
            }

            // add & update
            foreach (var spec in newFos.Specializations)
            {
                var existing = oldFos.Specializations.FirstOrDefault(s => s.Code == spec.Code);
                if (existing != null)
                {
                    existing.Name = spec.Name;
                }
                else
                {
                    oldFos.Specializations.Add(spec);
                }
            }
        }

        public override async Task<bool> DeleteAsync(string id)
        {
            FieldOfStudy fos = await base.GetByCodeAsync(id);
            _dbContext.Specializations.RemoveRange(fos.Specializations);
            EntityEntry<FieldOfStudy> result = _dbSet.Remove(fos);

            await _dbContext.SaveChangesAsync();

            if (result.State == EntityState.Deleted)
                return true;

            return false;

        }

        public override async Task<bool> SoftDeleteAsync(string id)
        {
            FieldOfStudy fos = await GetByCodeAsync(id);
            if (fos == null)
                return false;

            fos.Specializations.ForEach(s => s.IsDeleted = true);
            fos.IsDeleted = true;

            await _dbContext.SaveChangesAsync();

            return true;

        }

        public override async Task<FieldOfStudy> GetByCodeAsync(string id)
        {
            return await _dbSet.Where(e => e.Code == id).Include(e => e.Specializations).FirstOrDefaultAsync();
        }
    }

}
