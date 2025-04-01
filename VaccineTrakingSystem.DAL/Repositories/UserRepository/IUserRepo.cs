using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.UserRepository
{
    public interface IUserRepo : IGenericRepository<User>
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
