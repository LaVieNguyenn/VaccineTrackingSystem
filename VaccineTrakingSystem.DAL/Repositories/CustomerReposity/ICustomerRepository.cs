using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.CustomerReposity
{
    public interface ICustomerRepository
    {
        Task<int> RegisterCustomerAsync(User user, List<Child> children);
    }
}
