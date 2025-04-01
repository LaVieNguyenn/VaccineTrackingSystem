using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.UserDAO
{
    public interface IUserDAO : IGenericDAO<User>
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
