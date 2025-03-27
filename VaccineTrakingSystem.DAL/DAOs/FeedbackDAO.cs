using System.Data;
using System.Data.SqlClient;
using Dapper;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class FeedbackDAO : IGenericDAO<Feedback>
    {
        private readonly string _connectionString;

        public FeedbackDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT f.FeedbackID AS FeedbackId,
                                  f.CustomerID AS CustomerId,
                                  f.AppointmentID AS AppointmentId,
                                  f.Rating,
                                  f.Comment,
                                  f.FeedbackDate,
                                  f.CreatedAt,
                                  f.UpdatedAt,
                                  u.UserID AS UserId,
                                  u.Username,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  u.Address,
                                  u.RoleId,
                                  u.CreatedAt AS UserCreatedAt,
                                  u.UpdatedAt AS UserUpdatedAt
                           FROM Feedbacks f
                           INNER JOIN Users u ON f.CustomerID = u.UserID
                           ORDER BY f.FeedbackDate DESC";
                return await connection.QueryAsync<Feedback, User, Feedback>(sql,
                    (feedback, user) =>
                    {
                        feedback.User = user;
                        return feedback;
                    },
                    splitOn: "UserId");
            }
        }

        public async Task<Feedback?> GetByIdAsync(int feedbackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT f.FeedbackID AS FeedbackId,
                                  f.CustomerID AS CustomerId,
                                  f.AppointmentID AS AppointmentId,
                                  f.Rating,
                                  f.Comment,
                                  f.FeedbackDate,
                                  f.CreatedAt,
                                  f.UpdatedAt,
                                  u.UserID AS UserId,
                                  u.Username,
                                  u.FullName,
                                  u.Email,
                                  u.PhoneNumber,
                                  u.Address,
                                  u.RoleId,
                                  u.CreatedAt AS UserCreatedAt,
                                  u.UpdatedAt AS UserUpdatedAt
                           FROM Feedbacks f
                           INNER JOIN Users u ON f.CustomerID = u.UserID
                           WHERE f.FeedbackID = @FeedbackId";
                var result = await connection.QueryAsync<Feedback, User, Feedback>(sql,
                    (feedback, user) =>
                    {
                        feedback.User = user;
                        return feedback;
                    },
                    new { FeedbackId = feedbackId },
                    splitOn: "UserId");
                return result.FirstOrDefault();
            }
        }

        public async Task<int> InsertAsync(Feedback feedback)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Feedbacks (CustomerID, AppointmentID, Rating, Comment, FeedbackDate, CreatedAt, UpdatedAt)
                    VALUES (@CustomerId, @AppointmentId, @Rating, @Comment, @FeedbackDate, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, feedback);
            }
        }

        public async Task<bool> UpdateAsync(Feedback feedback)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE Feedbacks
                    SET CustomerID = @CustomerId,
                        AppointmentID = @AppointmentId,
                        Rating = @Rating,
                        Comment = @Comment,
                        FeedbackDate = @FeedbackDate,
                        UpdatedAt = @UpdatedAt
                    WHERE FeedbackID = @FeedbackId";
                var affectedRows = await connection.ExecuteAsync(sql, feedback);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int feedbackId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Feedbacks WHERE FeedbackID = @FeedbackId";
                var affectedRows = await connection.ExecuteAsync(sql, new { FeedbackId = feedbackId });
                return affectedRows > 0;
            }
        }
    }
} 