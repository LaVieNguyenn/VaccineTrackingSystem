using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.VaccineService
{
    public class VaccineService : IVaccineService
    {
        private readonly IGenericRepository<Vaccine> _repository;

        public VaccineService(IGenericRepository<Vaccine> repository)
        {
            _repository = repository;
        }
        public Task<int> CreateVaccineServiceAsync(Vaccine vaccine)
        {
            return _repository.InsertAsync(vaccine);
        }

        public Task<bool> DeleteVaccineServiceAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<Vaccine>> GetAllVaccineServiceAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Vaccine?> GetVaccineServiceByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateVaccineServiceAsync(Vaccine vaccine)
        {
            return _repository.UpdateAsync(vaccine);
        }
    }
}
