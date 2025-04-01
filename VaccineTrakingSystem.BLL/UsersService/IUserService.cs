using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.UsersService
{
    public interface IUsersService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> DeleteUserAsync(int id);
        Task<int> CreateUsersAsync(User user);

    }
}
