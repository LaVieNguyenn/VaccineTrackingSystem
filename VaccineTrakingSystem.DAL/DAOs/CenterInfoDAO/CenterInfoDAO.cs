using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class CenterInfoDAO : IGenericDAO<CenterInfo>
    {
        private readonly string _connectionString;

        public CenterInfoDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<CenterInfo>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT CenterID AS CenterId, 
                                   CenterName, 
                                   Address, 
                                   ContactInfo, 
                                   Description, 
                                   ServiceDetails, 
                                   CreatedAt, 
                                   UpdatedAt 
                            FROM CenterInfo 
                            ORDER BY CreatedAt ASC";
                return await connection.QueryAsync<CenterInfo>(sql);
            }
        }

        public async Task<CenterInfo?> GetByIdAsync(int centerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT CenterID AS CenterId, 
                                   CenterName, 
                                   Address, 
                                   ContactInfo, 
                                   Description, 
                                   ServiceDetails, 
                                   CreatedAt, 
                                   UpdatedAt 
                            FROM CenterInfo 
                            WHERE CenterID = @CenterId";
                return await connection.QueryFirstOrDefaultAsync<CenterInfo>(sql, new { CenterId = centerId });
            }
        }

        public async Task<int> InsertAsync(CenterInfo centerInfo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var currentTime = DateTime.Now;
                centerInfo.CreatedAt = currentTime;
                centerInfo.UpdatedAt = currentTime;

                var sql = @"
            INSERT INTO CenterInfo (CenterName, Address, ContactInfo, Description, ServiceDetails, CreatedAt, UpdatedAt)
            VALUES (@CenterName, @Address, @ContactInfo, @Description, @ServiceDetails, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(sql, centerInfo);
            }
        }

        public async Task<bool> UpdateAsync(CenterInfo centerInfo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    UPDATE CenterInfo
                    SET CenterName = @CenterName,
                        Address = @Address,
                        ContactInfo = @ContactInfo,
                        Description = @Description,
                        ServiceDetails = @ServiceDetails,
                        UpdatedAt = @UpdatedAt
                    WHERE CenterID = @CenterId";
                var affectedRows = await connection.ExecuteAsync(sql, centerInfo);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int centerId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM CenterInfo WHERE CenterID = @CenterId";
                var affectedRows = await connection.ExecuteAsync(sql, new { CenterId = centerId });
                return affectedRows > 0;
            }
        }
    }
}
