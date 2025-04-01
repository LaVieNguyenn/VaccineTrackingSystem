using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.CenterInfoService
{
    public class CenterInfoService : ICenterInfoService
    {
        private readonly IGenericRepository<CenterInfo> _repository;

        public CenterInfoService(IGenericRepository<CenterInfo> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<CenterInfo>> GetAllCenterInfoAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<CenterInfo?> GetCenterInfoByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<int> CreateCenterInfoAsync(CenterInfo service)
        {
            return _repository.InsertAsync(service);
        }

        public Task<bool> UpdateCenterInfoAsync(CenterInfo service)
        {
            return _repository.UpdateAsync(service);
        }

        public Task<bool> DeleteCenterInfoAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
