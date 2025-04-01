using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.VaccineScheduleService
{
    public interface IVaccineScheduleService
    {
        Task<IEnumerable<VaccineSchedule>> GetAllVaccineScheduleServiceAsync();
        Task<VaccineSchedule?> GetVaccineScheduleServiceByIdAsync(int id);
        Task<int> CreateVaccineScheduleServiceAsync(VaccineSchedule vaccineSchedule);
        Task<bool> UpdateVaccineScheduleServiceAsync(VaccineSchedule vaccineSchedule);
        Task<bool> DeleteVaccineScheduleServiceAsync(int id);
    }
}
