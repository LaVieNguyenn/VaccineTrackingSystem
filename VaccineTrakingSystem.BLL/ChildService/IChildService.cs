using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.Services
{
    public interface IChildService
    {
        Task<IEnumerable<Child>> GetAllChildrenAsync();
        Task<Child?> GetChildByIdAsync(int id);
        Task<int> AddChildAsync(Child child);
        Task<bool> UpdateChildAsync(Child child);
        Task<bool> DeleteChildAsync(int id);

        Task<IEnumerable<Child>> GetChildrenByParentIdAsync(int parentId); // Thêm phương thức mới    }
    }
}