using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.Services
{
    public class ChildService : IChildService
    {
        private readonly IGenericRepository<Child> _repository;

        public ChildService(IGenericRepository<Child> repository)
        {
            _repository = repository;
        }

        public async Task<int> AddChildAsync(Child child)
        {
            return await _repository.InsertAsync(child);
        }

        public Task<IEnumerable<Child>> GetAllChildrenAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Child?> GetChildByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateChildAsync(Child child)
        {
            return await _repository.UpdateAsync(child);
        }

        public Task<bool> DeleteChildAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Child>> GetChildrenByParentIdAsync(int parentId)
        {
            var allChildren = await _repository.GetAllAsync();
            return allChildren.Where(child => child.ParentId == parentId).ToList();
        }
    }
}
