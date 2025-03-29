using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.BLL.Services;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.AppointmentService
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _repository;

        public AppointmentService(IGenericRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public Task<int> CreateAppointmentAsync(Appointment appointment)
        {
            return _repository.InsertAsync(appointment);
        }

        public Task<bool> DeleteAppointmentAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<IEnumerable<Appointment>> GetAllAppointmentsAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<Appointment?> GetAppointmentByIdAsync(int id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            return _repository.UpdateAsync(appointment);
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByChildIdAsync(int childId)
        {
            var appointments = await _repository.GetAllAsync();
            return appointments.Where(a => a.ChildId == childId)
                               .OrderByDescending(a => a.AppointmentDate);
        }
    }
}
