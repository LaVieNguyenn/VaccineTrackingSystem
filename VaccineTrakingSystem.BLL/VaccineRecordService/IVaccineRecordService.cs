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
        Task<IEnumerable<VaccinationRecord>> GetAllVaccineRecordServiceAsync();
        Task<VaccinationRecord?> GetVaccineRecordServiceByIdAsync(int id);
        Task<int> CreateVaccineRecordServiceAsync(VaccinationRecord vaccinationRecord);
        Task<bool> UpdateVaccineRecordServiceAsync(VaccinationRecord vaccinationRecord);
        Task<bool> DeleteVaccineRecordServiceAsync(int id);
    }
}
