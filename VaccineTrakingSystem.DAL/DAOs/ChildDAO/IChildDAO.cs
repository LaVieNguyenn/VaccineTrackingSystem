using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.ChildDAO
{
    public interface IChildDAO : IGenericDAO<Child>
    {
        Task<IEnumerable<Child>> GetAllAsync();
        Task<Child?> GetByIdAsync(int id);
        Task<int> InsertChildAsync(Child child);
        Task<bool> UpdateAsync(Child child);
        Task<bool> DeleteAsync(int id);
    }
}
