using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.VaccineService
{
    public interface IVaccineService
    {
        Task<IEnumerable<Vaccine>> GetAllVaccineServiceAsync();
        Task<Vaccine?> GetVaccineServiceByIdAsync(int id);
        Task<int> CreateVaccineServiceAsync(Vaccine vaccine);
        Task<bool> UpdateVaccineServiceAsync(Vaccine vaccine);
        Task<bool> DeleteVaccineServiceAsync(int id);
    }
}
