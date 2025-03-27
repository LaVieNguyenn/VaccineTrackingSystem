using System.Data;
using System.Data.SqlClient;
using Dapper;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class NotificationDAO : IGenericDAO<Notification>
    {
        private readonly string _connectionString;

        public NotificationDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Notification>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT n.NotificationID AS NotificationId,
                                  n.UserID AS UserId,
                                  n.ChildID AS ChildId,
                                  n.AppointmentID AS AppointmentId,
                                  n.Message,
                                  n.NotificationDate,
                                  n.IsSent,
                                  n.CreatedAt,
                                  n.UpdatedAt,
                                  u.UserID AS UserId,
                                  u.Username,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  u.Address,
                                  u.RoleId,
                                  u.CreatedAt AS UserCreatedAt,
                                  u.UpdatedAt AS UserUpdatedAt
                           FROM Notifications n
                           INNER JOIN Users u ON n.UserID = u.UserID
                           ORDER BY n.NotificationDate DESC";
                return await connection.QueryAsync<Notification, User, Notification>(sql,
                    (notification, user) =>
                    {
                        notification.User = user;
                        return notification;
                    },
                    splitOn: "UserId");
            }
        }

        public async Task<Notification?> GetByIdAsync(int notificationId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT n.NotificationID AS NotificationId,
                                  n.UserID AS UserId,
                                  n.ChildID AS ChildId,
                                  n.AppointmentID AS AppointmentId,
                                  n.Message,
                                  n.NotificationDate,
                                  n.IsSent,
                                  n.CreatedAt,
                                  n.UpdatedAt,
                                  u.UserID AS UserId,
                                  u.Username,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  u.Address,
                                  u.RoleId,
                                  u.CreatedAt AS UserCreatedAt,
                                  u.UpdatedAt AS UserUpdatedAt
                           FROM Notifications n
                           INNER JOIN Users u ON n.UserID = u.UserID
                           WHERE n.NotificationID = @NotificationId";
                var result = await connection.QueryAsync<Notification, User, Notification>(sql,
                    (notification, user) =>
                    {
                        notification.User = user;
                        return notification;
                    },
                    new { NotificationId = notificationId },
                    splitOn: "UserId");
                return result.FirstOrDefault();
            }
        }

        public async Task<int> InsertAsync(Notification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Notifications (UserID, ChildID, AppointmentID, Message, NotificationDate, IsSent, CreatedAt, UpdatedAt)
                    VALUES (@UserId, @ChildId, @AppointmentId, @Message, @NotificationDate, @IsSent, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, notification);
            }
        }

        public async Task<bool> UpdateAsync(Notification notification)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE Notifications
                    SET UserID = @UserId,
                        ChildID = @ChildId,
                        AppointmentID = @AppointmentId,
                        Message = @Message,
                        NotificationDate = @NotificationDate,
                        IsSent = @IsSent,
                        UpdatedAt = @UpdatedAt
                    WHERE NotificationID = @NotificationId";
                var affectedRows = await connection.ExecuteAsync(sql, notification);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int notificationId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Notifications WHERE NotificationID = @NotificationId";
                var affectedRows = await connection.ExecuteAsync(sql, new { NotificationId = notificationId });
                return affectedRows > 0;
            }
        }
    }
} 