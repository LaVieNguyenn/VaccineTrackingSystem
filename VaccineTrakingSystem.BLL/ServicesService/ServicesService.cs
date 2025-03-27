using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.ServicesService
{
    public class ServicesService : IServicesService
    {
        private readonly IGenericRepository<Service> _repository;

        public ServicesService(IGenericRepository<Service> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<Service>> GetAllServicesAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Service?> GetServiceByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> CreateServiceAsync(Service service)
        {
            return _repository.InsertAsync(service);
        }

        public Task<bool> UpdateServiceAsync(Service service)
        {
            return _repository.UpdateAsync(service);
        }

        public Task<bool> DeleteServiceAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
