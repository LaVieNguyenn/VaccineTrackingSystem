using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.DashboardDao
{
    public interface IDashboardDAO
    {
        Task<int> GetTotalAppointmentsAsync();
        Task<IEnumerable<dynamic>> GetAppointmentStatusDistributionAsync();
        Task<IEnumerable<Appointment>> GetLatestAppointmentsAsync(int count);
        Task<int> GetTotalCustomersAsync();
        Task<IEnumerable<User>> GetLatestCustomersAsync(int count);
        Task<int> GetTotalVaccinationRecordsAsync();

    }
}
