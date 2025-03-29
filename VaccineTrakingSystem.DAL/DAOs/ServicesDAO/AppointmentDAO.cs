using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class AppointmentDAO : IGenericDAO<Appointment>
    {
        private readonly string _connectionString;

        public AppointmentDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Appointment>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"SELECT * FROM Appointments ORDER BY BookingDate DESC";
            return await connection.QueryAsync<Appointment>(sql);
        }

        public async Task<Appointment?> GetByIdAsync(int appointmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM Appointments WHERE AppointmentId = @AppointmentId";
            return await connection.QueryFirstOrDefaultAsync<Appointment>(sql, new { AppointmentId = appointmentId });
        }

        public async Task<int> InsertAsync(Appointment appointment)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                INSERT INTO Appointments (ChildId, ServiceId, AppointmentDate, BookingDate, Status, PaymentStatus, CreatedAt, UpdatedAt)
                VALUES (@ChildId, @ServiceId, @AppointmentDate, @BookingDate, @Status, @PaymentStatus, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await connection.QuerySingleAsync<int>(sql, appointment);
        }

        public async Task<bool> UpdateAsync(Appointment appointment)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                UPDATE Appointments
                SET ChildId = @ChildId,
                    ServiceId = @ServiceId,
                    AppointmentDate = @AppointmentDate,
                    BookingDate = @BookingDate,
                    Status = @Status,
                    PaymentStatus = @PaymentStatus,
                    UpdatedAt = @UpdatedAt
                WHERE AppointmentId = @AppointmentId";
            var affectedRows = await connection.ExecuteAsync(sql, appointment);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int appointmentId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "DELETE FROM Appointments WHERE AppointmentId = @AppointmentId";
            var affectedRows = await connection.ExecuteAsync(sql, new { AppointmentId = appointmentId });
            return affectedRows > 0;
        }
    }
}
