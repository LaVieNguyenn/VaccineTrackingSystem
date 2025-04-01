using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.DTOs;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories.CustomerReposity;

namespace VaccineTrakingSystem.BLL.CustomerService
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<int> RegisterCustomerAsync(CustomerRegisterModel model)
        {
            // Chuyển đổi CustomerRegisterViewModel sang model User
            var user = new User
            {
                Username = model.Username,
                // Ở production, băm mật khẩu trước khi lưu
                PasswordHash = model.Password,
                Email = model.Email,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                RoleId = 2, // Giả sử RoleID = 2 tương ứng với Customer
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            // Chuyển đổi danh sách Children
            var children = new List<Child>();
            foreach (var childVM in model.Children)
            {
                var child = new Child
                {
                    FullName = childVM.FullName,
                    DateOfBirth = childVM.DateOfBirth,
                    Gender = childVM.Gender,
                    AdditionalInfo = childVM.AdditionalInfo,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                children.Add(child);
            }

            return await _customerRepository.RegisterCustomerAsync(user, children);
        }
    }
}
