using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.Services.UserService
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
