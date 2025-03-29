using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.VaccineDAO
{
    public class VaccineDAO : IGenericDAO<Vaccine>
    {
        private readonly string _connectionString;

        public VaccineDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Vaccines WHERE VaccineID = @VaccineId";
                var affectedRows = await connection.ExecuteAsync(sql, new { VaccineId = id });
                return affectedRows > 0;
            }
        }

        public async Task<IEnumerable<Vaccine>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT VaccineID AS VaccineId, 
                                   VaccineName, 
                                   Description, 
                                   NumberOfDoses, 
                                   Manufacturer, 
                                   VaccineType, 
                                   ExpirationPeriod,
                                    ProductionDate,
                                    CreatedAt,
                                    UpdatedAt
                            FROM Vaccines 
                            ORDER BY CreatedAt DESC";
                return await connection.QueryAsync<Vaccine>(sql);
            }
        }

        public async Task<Vaccine?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"SELECT VaccineID AS VaccineId, 
                                   VaccineName, 
                                   Description, 
                                    NumberOfDoses, 
                                   Manufacturer, 
                                   VaccineType, 
                                   ExpirationPeriod,
                                    ProductionDate,
                                    CreatedAt,
                                    UpdatedAt
                            FROM Vaccines 
                            WHERE VaccineID = @VaccineId";
                return await connection.QueryFirstOrDefaultAsync<Vaccine>(sql, new { ServiceId = id });
            }
        }

        public async Task<int> InsertAsync(Vaccine obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                obj.CreatedAt = DateTime.UtcNow;
                obj.UpdatedAt = DateTime.UtcNow;
                var sql = @"
                    INSERT INTO Vaccines (VaccineName, Description, NumberOfDoses, Manufacturer,VaccineType,ExpirationPeriod,ProductionDate, CreatedAt, UpdatedAt)
                    VALUES (@VaccineName, @Description, @NumberOfDoses, @Manufacturer,@VaccineType,@ExpirationPeriod,@ProductionDate, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, obj);
            }
        }

        public async Task<bool> UpdateAsync(Vaccine obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                obj.UpdatedAt = DateTime.UtcNow;
                var sql = @"
                    UPDATE Vaccines
                    SET VaccineName = @ServiceName,
                        Description = @Description,
                        NumberOfDoses = @NumberOfDoses,
                        Manufacturer = @Manufacturer,
                        VaccineType = @VaccineType
                        ExpirationPeriod = @ExpirationPeriod,
                        ProductionDate = @ProductionDate,
                        UpdatedAt = @UpdatedAt
                    WHERE VaccineID = @VaccineId";
                var affectedRows = await connection.ExecuteAsync(sql, obj);
                return affectedRows > 0;
            }
        }
    }
}
