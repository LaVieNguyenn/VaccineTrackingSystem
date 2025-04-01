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

                var result = await connection.QueryAsync(sql, (Vaccine v, DateTime productionDate) =>
                {
                    v.ProductionDate = DateOnly.FromDateTime(productionDate); // ✅ Convert DateTime -> DateOnly
                    return v;
                },
                splitOn: "ProductionDate"); // ⚡ Quan trọng: Cho Dapper biết đây là column cần map

                // 🔥 Log dữ liệu sau khi query từ DB
                foreach (var v in result)
                {
                    Console.WriteLine($"ID: {v.VaccineId}, CreatedAt: {v.CreatedAt}, UpdatedAt: {v.UpdatedAt}");
                }

                return result;
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
                                    ProductionDate
                            FROM Vaccines 
                            WHERE VaccineID = @VaccineId";
                return await connection.QueryFirstOrDefaultAsync<Vaccine>(sql, new { VaccineId = id });
            }
        }

        public async Task<int> InsertAsync(Vaccine obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
            INSERT INTO Vaccines (VaccineName, Description, NumberOfDoses, Manufacturer, VaccineType, ExpirationPeriod, ProductionDate, CreatedAt, UpdatedAt)
            VALUES (@VaccineName, @Description, @NumberOfDoses, @Manufacturer, @VaccineType, @ExpirationPeriod, @ProductionDate, @CreatedAt, @UpdatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

                var param = new
                {
                    obj.VaccineName,
                    obj.Description,
                    obj.NumberOfDoses,
                    obj.Manufacturer,
                    obj.VaccineType,
                    obj.ExpirationPeriod,
                    ProductionDate = obj.ProductionDate.HasValue
                        ? obj.ProductionDate.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null, // 🔥 Convert DateOnly? → DateTime?
                    obj.CreatedAt,
                    obj.UpdatedAt
                };

                return await connection.QuerySingleAsync<int>(sql, param);
            }
        }


        public async Task<bool> UpdateAsync(Vaccine model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var productionDate = model.ProductionDate.HasValue
                    ? model.ProductionDate.Value.ToDateTime(TimeOnly.MinValue)
                    : (DateTime?)null;

                var parameters = new
                {
                    VaccineId = model.VaccineId,
                    VaccineName = model.VaccineName,
                    Description = model.Description,
                    NumberOfDoses = model.NumberOfDoses,
                    Manufacturer = model.Manufacturer,
                    VaccineType = model.VaccineType,
                    ExpirationPeriod = model.ExpirationPeriod,
                    ProductionDate = productionDate,
                    UpdatedAt = DateTime.UtcNow // Cập nhật thời gian sửa
                };

                var sql = @"
            UPDATE Vaccines
            SET VaccineName = @VaccineName,
                Description = @Description,
                NumberOfDoses = @NumberOfDoses,
                Manufacturer = @Manufacturer,
                VaccineType = @VaccineType,
                ExpirationPeriod = @ExpirationPeriod,
                ProductionDate = @ProductionDate,
                UpdatedAt = @UpdatedAt
            WHERE VaccineID = @VaccineId";

                var affectedRows = await connection.ExecuteAsync(sql, parameters);
                return affectedRows > 0;
            }
        }

    }
}


