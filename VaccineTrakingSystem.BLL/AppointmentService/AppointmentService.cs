using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;
using VaccineTrakingSystem.DAL.Repositories;

namespace VaccineTrakingSystem.BLL.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IGenericRepository<Appointment> _repository;

        public AppointmentService(IGenericRepository<Appointment> repository)
        {
            _repository = repository;
        }

        public async Task<int> BookAppointmentAsync(Appointment appointment)
        {
            try
            {
                // Kiểm tra và gán giá trị hợp lệ nếu cần
                if (appointment.CreatedAt == default || appointment.CreatedAt < new DateTime(1753, 1, 1))
                {
                    appointment.CreatedAt = DateTime.UtcNow;
                    Console.WriteLine("CreatedAt was invalid, set to UtcNow");
                }
                if (appointment.UpdatedAt == default || appointment.UpdatedAt < new DateTime(1753, 1, 1))
                {
                    appointment.UpdatedAt = DateTime.UtcNow;
                    Console.WriteLine("UpdatedAt was invalid, set to UtcNow");
                }

                return await _repository.InsertAsync(appointment);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BookAppointmentAsync: {ex.Message}");
                throw;
            }
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

        public Task<bool> CancelAppointmentAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }
        public async Task<IEnumerable<Appointment>> GetAppointmentsByChildIdAsync(int childId)
        {
            var allAppointments = await _repository.GetAllAsync();
            return allAppointments.Where(a => a.ChildId == childId).ToList();
        }
    }
}
