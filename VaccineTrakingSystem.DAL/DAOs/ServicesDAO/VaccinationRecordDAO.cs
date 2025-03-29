using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class VaccinationRecordDAO : IGenericDAO<VaccinationRecord>
    {
        private readonly string _connectionString;

        public VaccinationRecordDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<VaccinationRecord>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM VaccinationRecords ORDER BY VaccinationDate DESC";
            return await connection.QueryAsync<VaccinationRecord>(sql);
        }

        public async Task<VaccinationRecord?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM VaccinationRecords WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<VaccinationRecord>(sql, new { Id = id });
        }

        public async Task<int> InsertAsync(VaccinationRecord record)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                INSERT INTO VaccinationRecords (ChildId, VaccineName, VaccinationDate, DoseNumber, Notes, CreatedAt, UpdatedAt)
                VALUES (@ChildId, @VaccineName, @VaccinationDate, @DoseNumber, @Notes, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await connection.QuerySingleAsync<int>(sql, record);
        }

        public async Task<bool> UpdateAsync(VaccinationRecord record)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                UPDATE VaccinationRecords
                SET ChildId = @ChildId,
                    VaccineName = @VaccineName,
                    VaccinationDate = @VaccinationDate,
                    DoseNumber = @DoseNumber,
                    Notes = @Notes,
                    UpdatedAt = @UpdatedAt
                WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, record);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "DELETE FROM VaccinationRecords WHERE Id = @Id";
            var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
            return affectedRows > 0;
        }
    }
}