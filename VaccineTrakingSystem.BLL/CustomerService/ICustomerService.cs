using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.DTOs;

namespace VaccineTrakingSystem.BLL.CustomerService
{
    public interface ICustomerService
    {
        Task<int> RegisterCustomerAsync(CustomerRegisterModel model);
    }
}
