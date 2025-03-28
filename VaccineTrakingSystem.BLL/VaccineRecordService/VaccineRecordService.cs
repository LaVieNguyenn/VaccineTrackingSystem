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
        public Task<int> CreateVaccineRecordServiceAsync(VaccinationRecord vaccinationRecord)
        {
            return _repository.InsertAsync(vaccinationRecord);
        }

        public Task<bool> DeleteVaccineRecordServiceAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<VaccinationRecord>> GetAllVaccineRecordServiceAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<VaccinationRecord?> GetVaccineRecordServiceByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateVaccineRecordServiceAsync(VaccinationRecord vaccinationRecord)
        {
            return _repository.UpdateAsync(vaccinationRecord);
        }
    }
}
