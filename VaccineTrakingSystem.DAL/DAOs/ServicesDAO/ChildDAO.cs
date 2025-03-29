using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs
{
    public class ChildDAO : IGenericDAO<Child>
    {
        private readonly string _connectionString;

        public ChildDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")!;
        }

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM Children ORDER BY CreatedAt DESC";
            return await connection.QueryAsync<Child>(sql);
        }

        public async Task<Child?> GetByIdAsync(int childId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "SELECT * FROM Children WHERE ChildID = @ChildId";
            return await connection.QueryFirstOrDefaultAsync<Child>(sql, new { ChildId = childId });
        }

        public async Task<int> InsertAsync(Child child)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                INSERT INTO Children (ParentID, FullName, DateOfBirth, Gender, AdditionalInfo, CreatedAt, UpdatedAt)
                VALUES (@ParentID, @FullName, @DateOfBirth, @Gender, @AdditionalInfo, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";
            return await connection.QuerySingleAsync<int>(sql, child);
        }

        public async Task<bool> UpdateAsync(Child child)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = @"
                UPDATE Children
                SET ParentID = @ParentID,
                    FullName = @FullName,
                    DateOfBirth = @DateOfBirth,
                    Gender = @Gender,
                    AdditionalInfo = @AdditionalInfo,
                    UpdatedAt = @UpdatedAt
                WHERE ChildID = @ChildID";
            var affectedRows = await connection.ExecuteAsync(sql, child);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteAsync(int childId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            var sql = "DELETE FROM Children WHERE ChildID = @ChildId";
            var affectedRows = await connection.ExecuteAsync(sql, new { ChildId = childId });
            return affectedRows > 0;
        }
    }
}
