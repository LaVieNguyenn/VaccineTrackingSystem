using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.VaccinationRecordService
{
    public interface IVaccinationRecordService
    {
        Task<IEnumerable<VaccinationRecord>> GetVaccinationRecordsByChildIdAsync(int childId);
    }
}
