using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.CenterInfoService
{
    public interface ICenterInfoService
    {
        Task<IEnumerable<CenterInfo>> GetAllCenterInfoAsync();
        Task<CenterInfo?> GetCenterInfoByIdAsync(int id);
        Task<int> CreateCenterInfoAsync(CenterInfo service);
        Task<bool> UpdateCenterInfoAsync(CenterInfo service);
        Task<bool> DeleteCenterInfoAsync(int id);

    }
}
