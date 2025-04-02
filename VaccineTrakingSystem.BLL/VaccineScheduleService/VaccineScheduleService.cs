using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.VaccineScheduleService
{
    public class VaccineScheduleService : IVaccineScheduleService
    {

        private readonly IGenericRepository<VaccineSchedule> _repository;
        private readonly IGenericRepository<Vaccine> _vaccineRepository; // Thêm repository cho Vaccine
        public VaccineScheduleService(IGenericRepository<VaccineSchedule> repository, IGenericRepository<Vaccine> vaccineRepository)
        {
            _repository = repository;
            _vaccineRepository = vaccineRepository;
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

        public Task<int> CreateVaccineScheduleServiceAsyncc(VaccineSchedule vaccineSchedule)
        {
            return _repository.InsertAsyncc(vaccineSchedule);
        }
        public async Task<IEnumerable<VaccineSchedule>> GetVaccineSchedulesByChildIdAsync(int childId)
        {
            var allSchedules = await _repository.GetAllAsync();
            var filteredSchedules = allSchedules.Where(vs => vs.ChildId == childId).ToList();

            // Lấy dữ liệu Vaccine cho từng VaccineSchedule
            foreach (var schedule in filteredSchedules)
            {
                if (schedule.VaccineId > 0)
                {
                    schedule.Vaccine = await _vaccineRepository.GetByIdAsync(schedule.VaccineId);
                }
            }

            return filteredSchedules;
        }
    }
}
