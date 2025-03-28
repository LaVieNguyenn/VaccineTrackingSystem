using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.VaccineRecordService
{
    public interface IVaccineRecordService
    {
        Task<IEnumerable<VaccinationRecord>> GetAllVaccineScheduleServiceAsync();
        Task<VaccinationRecord?> GetVaccineScheduleServiceByIdAsync(int id);
        Task<int> CreateVaccineScheduleServiceAsync(VaccinationRecord vaccinationRecord);
        Task<bool> UpdateVaccineScheduleServiceAsync(VaccinationRecord vaccinationRecord);
        Task<bool> DeleteVaccineScheduleServiceAsync(int id);
    }
}
