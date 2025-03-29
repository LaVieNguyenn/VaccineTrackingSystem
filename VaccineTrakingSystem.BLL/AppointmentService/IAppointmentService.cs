using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.BLL.Services
{
    public interface IAppointmentService
    {
        Task<IEnumerable<Appointment>> GetAllAppointmentsAsync();
        Task<Appointment?> GetAppointmentByIdAsync(int appointmentId);
        Task<int> CreateAppointmentAsync(Appointment appointment);
        Task<bool> UpdateAppointmentAsync(Appointment appointment);
        Task<bool> DeleteAppointmentAsync(int appointmentId);
        Task<IEnumerable<Appointment>> GetAppointmentsByChildIdAsync(int childId);

    }
}
