using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.FeedbackDAO
{
    public class FeedbackDAO : IFeedbackDAO
    {
        private readonly string _connectionString;

        public FeedbackDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Feedback WHERE FeedbackId = @FeedbackId";
                var affectedRows = await connection.ExecuteAsync(sql, new { FeedbackId = id });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Feedback>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT FeedbackId,
                                  AppointmentId,
                                  CustomerId,
                                  Rating,
                                  Comments,
                                  FeedbackDate,
                                  CreatedAt,
                                  UpdatedAt
                           FROM Feedback
                           ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Feedback>(sql);
            }
        }

        public async Task<Feedback?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT FeedbackId,
                                  AppointmentId,
                                  CustomerId,
                                  Rating,
                                  Comments,
                                  FeedbackDate,
                                  CreatedAt,
                                  UpdatedAt
                           FROM Feedback
                           WHERE FeedbackId = @FeedbackId";
                return await connection.QueryFirstOrDefaultAsync<Feedback>(sql, new { FeedbackId = id });
            }
        }

        public async Task<int> InsertAsync(Feedback obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"INSERT INTO Feedback (AppointmentId, CustomerId, Rating, Comments, FeedbackDate, CreatedAt, UpdatedAt)
                           VALUES (@AppointmentId, @CustomerId, @Rating, @Comments, @FeedbackDate, @CreatedAt, @UpdatedAt);
                           SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var parameters = new
                {
                    obj.AppointmentId,
                    obj.CustomerId,
                    obj.Rating,
                    obj.Comments,
                    obj.FeedbackDate,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                return await connection.QuerySingleAsync<int>(sql, parameters);
            }
        }

        public async Task<bool> UpdateAsync(Feedback model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"UPDATE Feedback
                           SET Rating = @Rating,
                               Comments = @Comments,
                               UpdatedAt = @UpdatedAt
                           WHERE FeedbackId = @FeedbackId";

                var parameters = new
                {
                    model.FeedbackId,
                    model.Rating,
                    model.Comments,
                    UpdatedAt = DateTime.UtcNow
                };

                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByCustomerId(int customerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT FeedbackId,
                                  AppointmentId,
                                  CustomerId,
                                  Rating,
                                  Comments,
                                  FeedbackDate,
                                  CreatedAt,
                                  UpdatedAt
                           FROM Feedback
                           WHERE CustomerId = @CustomerId
                           ORDER BY FeedbackDate DESC";
                return await connection.QueryAsync<Feedback>(sql, new { CustomerId = customerId });
            }
        }

        public async Task<IEnumerable<Feedback>> GetFeedbacksByAppointmentId(int appointmentId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT FeedbackId,
                                  AppointmentId,
                                  CustomerId,
                                  Rating,
                                  Comments,
                                  FeedbackDate,
                                  CreatedAt,
                                  UpdatedAt
                           FROM Feedback
                           WHERE AppointmentId = @AppointmentId
                           ORDER BY FeedbackDate DESC";
                return await connection.QueryAsync<Feedback>(sql, new { AppointmentId = appointmentId });
            }
        }

        public Task<int> InsertAsyncc(Feedback obj)
        {
            throw new NotImplementedException();
        }
    }
} 