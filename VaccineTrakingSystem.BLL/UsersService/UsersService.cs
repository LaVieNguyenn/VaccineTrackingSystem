using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs.UsersDAO;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.UsersService
{
    public class UsersService : IUsersService
    {
        private readonly IGenericRepository<User> _repository;

        public UsersService(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return _repository.GetAllAsync();
        }
        public Task<User?> GetUserByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> CreateUsersAsync(User user)
        {
            return _repository.InsertAsync(user);
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            return await _repository.UpdateAsync(user);
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
