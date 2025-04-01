using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs;

namespace VaccineTrakingSystem.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IGenericDAO<T> _dao;

        public GenericRepository(IGenericDAO<T> dao)
        {
            _dao = dao;
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            return _dao.GetAllAsync();
        }

        public Task<T?> GetByIdAsync(int id)
        {
            return _dao.GetByIdAsync(id);
        }

        public Task<int> InsertAsync(T entity)
        {
            return _dao.InsertAsync(entity);
        }

        public Task<bool> UpdateAsync(T entity)
        {
            return _dao.UpdateAsync(entity);
        }
        
        public Task<bool> DeleteAsync(int id)
        {
            return _dao.DeleteAsync(id);
        }
    }
}
