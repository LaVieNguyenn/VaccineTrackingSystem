using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs.UserDAO;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.UserRepository
{
    public class UserRepo : IUserRepo
    {
        private readonly IUserDAO _userDAO;

        public UserRepo(IUserDAO userDAO)
        {
            _userDAO = userDAO;
        }

        public Task<User?> AuthenticateAsync(string username, string password)
        {
            return  _userDAO.AuthenticateAsync(username, password);
        }

        public Task<bool> DeleteAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(int obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsync(User obj)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertAsyncc(User obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(User obj)
        {
            throw new NotImplementedException();
        }
    }
}
