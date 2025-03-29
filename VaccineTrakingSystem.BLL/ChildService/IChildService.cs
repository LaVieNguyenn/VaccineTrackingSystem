using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.ChildService
{
    public interface IChildService
    {
        Task<IEnumerable<Child>> GetAllChildrenAsync();
        Task<Child?> GetChildByIdAsync(int id);
        Task<int> CreateChildAsync(Child child);
        Task<bool> UpdateChildAsync(Child child);
        Task<bool> DeleteChildAsync(int id);
    }
}
