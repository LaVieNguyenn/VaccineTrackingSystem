using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.ChildService
{
    public class ChildService : IChildService
    {
        private readonly IGenericRepository<Child> _childRepository;
        public ChildService(IGenericRepository<Child> childRepository)
        {
            _childRepository = childRepository;
        }

        public Task<int> CreateChildAsync(Child child)
        {
            return _childRepository.InsertAsync(child);
        }

        public Task<bool> DeleteChildAsync(int id)
        {
            return _childRepository.DeleteAsync(id);
        }

        public Task<IEnumerable<Child>> GetAllChildrenAsync()
        {
            return _childRepository.GetAllAsync();
        }

        public Task<Child?> GetChildByIdAsync(int id)
        {
            return _childRepository.GetByIdAsync(id);
        }

        public Task<bool> UpdateChildAsync(Child child)
        {
            return _childRepository.UpdateAsync(child);
        }
    }
}
