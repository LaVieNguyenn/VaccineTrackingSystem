using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.DashboardDao
{
    public class DashboardDao : IDashboardDAO
    {
        private readonly string _connectionString;
        public DashboardDao(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> GetTotalAppointmentsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT COUNT(*) FROM Appointments";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<IEnumerable<dynamic>> GetAppointmentStatusDistributionAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT Status, COUNT(*) as Count 
                    FROM Appointments
                    GROUP BY Status";
                return await connection.QueryAsync(sql);
            }
        }

        public async Task<IEnumerable<Appointment>> GetLatestAppointmentsAsync(int count)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = $@"
                    SELECT TOP {count} *
                    FROM Appointments
                    ORDER BY BookingDate DESC";
                return await connection.QueryAsync<Appointment>(sql);
            }
        }
        public async Task<int> GetTotalCustomersAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Giả sử RoleID = 2 là Customer
                var sql = "SELECT COUNT(*) FROM Users WHERE RoleID = 2";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }

        public async Task<IEnumerable<User>> GetLatestCustomersAsync(int count)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // Giả sử có trường CreatedAt để xác định thời gian đăng ký
                var sql = $@"
                    SELECT TOP {count} *
                    FROM Users
                    WHERE RoleID = 2
                    ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<User>(sql);
            }
        }

        public async Task<int> GetTotalVaccinationRecordsAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT COUNT(*) FROM VaccinationRecords";
                return await connection.ExecuteScalarAsync<int>(sql);
            }
        }
    }
}
