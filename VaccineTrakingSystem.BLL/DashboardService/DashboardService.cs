using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories.DashboardRepository;

namespace VaccineTrakingSystem.BLL.DashboardService
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _repo = dashboardRepository;
        }
        public Task<int> GetTotalAppointmentsAsync() => _repo.GetTotalAppointmentsAsync();
        public Task<int> GetTotalCustomersAsync() => _repo.GetTotalCustomersAsync();
        public Task<int> GetTotalVaccinationRecordsAsync() => _repo.GetTotalVaccinationRecordsAsync();
        public Task<IEnumerable<dynamic>> GetAppointmentStatusDistributionAsync() => _repo.GetAppointmentStatusDistributionAsync();
        public Task<IEnumerable<Appointment>> GetLatestAppointmentsAsync(int count) => _repo.GetLatestAppointmentsAsync(count);
        public Task<IEnumerable<User>> GetLatestCustomersAsync(int count) => _repo.GetLatestCustomersAsync(count);

    }
}
