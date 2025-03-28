using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.VaccineScheduleService
{
    public class VaccineScheduleService : IVaccineScheduleService
    {

        private readonly IGenericRepository<VaccineSchedule> _repository;

        public VaccineScheduleService(IGenericRepository<VaccineSchedule> repository)
        {
            _repository = repository;
        }

        public Task<int> CreateVaccineScheduleServiceAsync(VaccineSchedule vaccineSchedule)
        {
            return _repository.InsertAsync(vaccineSchedule);
        }

        public Task<bool> DeleteVaccineScheduleServiceAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<VaccineSchedule>> GetAllVaccineScheduleServiceAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<VaccineSchedule?> GetVaccineScheduleServiceByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateVaccineScheduleServiceAsync(VaccineSchedule vaccineSchedule)
        {
            return _repository.UpdateAsync(vaccineSchedule);
        }
    }
}
