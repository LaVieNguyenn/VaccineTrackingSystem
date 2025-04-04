using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.DAOs.DashboardDao;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.Repositories.DashboardRepository
{
    public class DashboardRepository :IDashboardRepository
    {
        private readonly IDashboardDAO _dashboardDAO;
        public DashboardRepository(IDashboardDAO dashboardDAO)
        {
            _dashboardDAO = dashboardDAO;
        }
        public Task<int> GetTotalAppointmentsAsync() => _dashboardDAO.GetTotalAppointmentsAsync();
        public Task<IEnumerable<dynamic>> GetAppointmentStatusDistributionAsync() => _dashboardDAO.GetAppointmentStatusDistributionAsync();
        public Task<IEnumerable<Appointment>> GetLatestAppointmentsAsync(int count) => _dashboardDAO.GetLatestAppointmentsAsync(count);
        public Task<int> GetTotalCustomersAsync() => _dashboardDAO.GetTotalCustomersAsync();
        public Task<IEnumerable<User>> GetLatestCustomersAsync(int count) => _dashboardDAO.GetLatestCustomersAsync(count);
        public Task<int> GetTotalVaccinationRecordsAsync() => _dashboardDAO.GetTotalVaccinationRecordsAsync();


    }
}
