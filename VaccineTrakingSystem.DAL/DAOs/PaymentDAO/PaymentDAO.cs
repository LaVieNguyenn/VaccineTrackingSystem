using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.PaymentDAO
{
    public class PaymentDAO : IPaymentDAO
    {
        private readonly string _connectionString;
        public PaymentDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> InsertPaymentAsync(Payment payment)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Payments (AppointmentID, CustomerID, Amount, PaymentDate, PaymentMethod, PaymentStatus, CreatedAt, UpdatedAt)
                    VALUES (@AppointmentID, @CustomerID, @Amount, @PaymentDate, @PaymentMethod, @PaymentStatus, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, payment);
            }
        }

        public async Task<bool> UpdatePaymentStatusAsync(int paymentId, int status, DateTime paymentDate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE Payments 
                    SET PaymentStatus = @Status, PaymentDate = @PaymentDate, UpdatedAt = GETDATE()
                    WHERE PaymentID = @PaymentID";
                var affected = await connection.ExecuteAsync(sql, new { PaymentID = paymentId, Status = status, PaymentDate = paymentDate });
                return affected > 0;
            }
        }

        public async Task<Payment?> GetPaymentByAppointmentAsync(int appointmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT PaymentID, AppointmentID, CustomerID, Amount, PaymentDate, PaymentMethod, PaymentStatus, CreatedAt, UpdatedAt
                    FROM Payments
                    WHERE AppointmentID = @AppointmentID";
                return await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { AppointmentID = appointmentId });
            }

        }

        public async Task<Payment?> GetPaymentByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT PaymentID, AppointmentID, CustomerID, Amount, PaymentDate, PaymentMethod, PaymentStatus, CreatedAt, UpdatedAt
                    FROM Payments
                    WHERE PaymentID = @Id";
                return await connection.QueryFirstOrDefaultAsync<Payment>(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<AppointmentDTO>> GetUnpaidAppointmentsAsync(int? appointmentId, string? phoneNumber, string? username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT a.AppointmentID, a.AppointmentDate, a.BookingDate, a.Status, a.PaymentStatus,
                           u.Username, u.PhoneNumber, u.FullName
                    FROM Appointments a
                    INNER JOIN Children c ON a.ChildID = c.ChildID
                    INNER JOIN Users u ON c.ParentID = u.UserID
                    WHERE a.PaymentStatus = 1
                      AND (@AppointmentID IS NULL OR a.AppointmentID = @AppointmentID)
                      AND (@PhoneNumber IS NULL OR u.PhoneNumber LIKE '%' + @PhoneNumber + '%')
                      AND (@Username IS NULL OR u.Username LIKE '%' + @Username + '%')
                    ORDER BY a.AppointmentDate DESC";

                return await connection.QueryAsync<AppointmentDTO>(sql, new { AppointmentID = appointmentId, PhoneNumber = phoneNumber, Username = username });
            }
        }
    }
}