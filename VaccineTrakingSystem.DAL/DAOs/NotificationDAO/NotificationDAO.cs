using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.NotificationDAO
{
    public class NotificationDAO : INotificationDAO
    {
        private readonly string _connectionString;
        public NotificationDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<int> InsertNotificationAsync(Notification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Notifications (UserID, ChildID, Message, NotificationDate, IsSent, CreatedAt, UpdatedAt)
                    VALUES (@UserId, @ChildId, @Message, @NotificationDate, @IsSent, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, notification);
            }
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserAsync(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "SELECT * FROM Notifications WHERE UserID = @UserId ORDER BY NotificationDate DESC";
                return await connection.QueryAsync<Notification>(sql, new { UserId = userId });
            }
        }

    }
}
