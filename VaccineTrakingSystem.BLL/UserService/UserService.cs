using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories.UserRepository;

namespace VaccineTrakingSystem.BLL.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;

        public UserService(IUserRepo repo)
        {
            _userRepo = repo;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            return await _userRepo.AuthenticateAsync(username, password);
        }
    }
}
