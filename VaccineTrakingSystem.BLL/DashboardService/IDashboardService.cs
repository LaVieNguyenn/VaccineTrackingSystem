using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.DashboardService
{
    public interface IDashboardService
    {
        Task<int> GetTotalAppointmentsAsync();
        Task<int> GetTotalCustomersAsync();
        Task<int> GetTotalVaccinationRecordsAsync();
        Task<IEnumerable<dynamic>> GetAppointmentStatusDistributionAsync();
        Task<IEnumerable<Appointment>> GetLatestAppointmentsAsync(int count);
        Task<IEnumerable<User>> GetLatestCustomersAsync(int count);
    }
}
