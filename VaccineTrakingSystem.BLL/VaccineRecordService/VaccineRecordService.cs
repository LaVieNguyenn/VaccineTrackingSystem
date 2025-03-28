using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.VaccineRecordService
{
    public class VaccineRecordService : IVaccineRecordService
    {
        private readonly IGenericRepository<VaccinationRecord> _repository;

        public VaccineRecordService(IGenericRepository<VaccinationRecord> repository)
        {
            _repository = repository;
        }
        public Task<int> CreateVaccineScheduleServiceAsync(VaccinationRecord vaccinationRecord)
        {
            return _repository.InsertAsync(vaccinationRecord);
        }

        public Task<bool> DeleteVaccineScheduleServiceAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<VaccinationRecord>> GetAllVaccineScheduleServiceAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<VaccinationRecord?> GetVaccineScheduleServiceByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateVaccineScheduleServiceAsync(VaccinationRecord vaccinationRecord)
        {
            return _repository.UpdateAsync(vaccinationRecord);
        }
    }
}
