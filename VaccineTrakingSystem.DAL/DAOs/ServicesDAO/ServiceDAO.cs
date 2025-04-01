using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.ServicesDAO
{
    public class ServiceDAO : IGenericDAO<Service>
    {
        private readonly string _connectionString;

        public ServiceDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        // Lấy danh sách tất cả các service
        public async Task<IEnumerable<Service>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT ServiceID AS ServiceId, 
                                   ServiceName, 
                                   Description, 
                                   Price, 
                                   Status, 
                                   CreatedAt, 
                                   UpdatedAt 
                            FROM Services 
                            ORDER BY CreatedAt ASC";
                return await connection.QueryAsync<Service>(sql);
            }
        }

        // Lấy service theo ID
        public async Task<Service?> GetByIdAsync(int serviceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT ServiceID AS ServiceId, 
                                   ServiceName, 
                                   Description, 
                                   Price, 
                                   Status, 
                                   CreatedAt, 
                                   UpdatedAt 
                            FROM Services 
                            WHERE ServiceID = @ServiceId";
                return await connection.QueryFirstOrDefaultAsync<Service>(sql, new { ServiceId = serviceId });
            }
        }

        // Thêm mới service
        public async Task<int> InsertAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var currentTime = DateTime.Now;
                service.CreatedAt = currentTime;
                service.UpdatedAt = currentTime;

                var sql = @"
            INSERT INTO Services (ServiceName, Description, Price, Status, CreatedAt, UpdatedAt)
            VALUES (@ServiceName, @Description, @Price, @Status, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(sql, service);
            }
        }
        // Cập nhật service
        public async Task<bool> UpdateAsync(Service service)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE Services
                    SET ServiceName = @ServiceName,
                        Description = @Description,
                        Price = @Price,
                        Status = @Status,
                        UpdatedAt = @UpdatedAt
                    WHERE ServiceID = @ServiceId";
                var affectedRows = await connection.ExecuteAsync(sql, service);
                return affectedRows > 0;
            }
        }

        // Xóa service theo ID
        public async Task<bool> DeleteAsync(int serviceId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Services WHERE ServiceID = @ServiceId";
                var affectedRows = await connection.ExecuteAsync(sql, new { ServiceId = serviceId });
                return affectedRows > 0;
            }
        }
    }
}
