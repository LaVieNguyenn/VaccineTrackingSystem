using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public interface IGenericDAO<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int obj);
        Task<int> InsertAsync(T obj);
        Task<bool> UpdateAsync(T obj);
        Task<bool> DeleteAsync(int obj); 
    }
}
