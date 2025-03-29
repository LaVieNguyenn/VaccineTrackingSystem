using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.VaccinationRecordService
{
    public class VaccinationRecordService : IVaccinationRecordService
    {
        private readonly IGenericRepository<VaccinationRecord> _repository;

        public VaccinationRecordService(IGenericRepository<VaccinationRecord> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<VaccinationRecord>> GetVaccinationRecordsByChildIdAsync(int childId)
        {
            var records = await _repository.GetAllAsync();
            return records.Where(r => r.ChildId == childId)
                          .OrderByDescending(r => r.VaccinationDate);
        }
    }
}
