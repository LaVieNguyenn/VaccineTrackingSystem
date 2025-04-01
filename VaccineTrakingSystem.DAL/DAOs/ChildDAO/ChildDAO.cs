using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VaccineTrakingSystem.DAL.Models;

namespace VaccineTrakingSystem.DAL.DAOs.ChildDAO
{
    public class ChildDAO : IChildDAO
    {
        private readonly string _connectionString;
        public ChildDAO(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            SqlMapper.AddTypeHandler(new DateOnlyTypeHandler());
        }

        public async Task<int> InsertChildAsync(Child child)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    INSERT INTO Children (ParentID, FullName, DateOfBirth, Gender, AdditionalInfo, CreatedAt, UpdatedAt)
                    VALUES (@ParentID, @FullName, @DateOfBirth, @Gender, @AdditionalInfo, @CreatedAt, @UpdatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                return await connection.QuerySingleAsync<int>(sql, child);
            }
        }
        public async Task<int> InsertAsync(Child child)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                INSERT INTO Children (ParentId, FullName, DateOfBirth, Gender, AdditionalInfo, CreatedAt, UpdatedAt) 
                VALUES (@ParentId, @FullName, @DateOfBirth, @Gender, @AdditionalInfo, @CreatedAt, @UpdatedAt);
                SELECT CAST(SCOPE_IDENTITY() AS INT);";

                return await connection.QuerySingleAsync<int>(sql, child);
            }
        }

        public async Task<IEnumerable<Child>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT 
                        ChildId,
                        ParentId,
                        FullName,
                        CAST(DateOfBirth AS DATE) AS DateOfBirth, -- Chỉ lấy phần ngày
                        Gender,
                        AdditionalInfo,
                        CreatedAt,
                        UpdatedAt
                    FROM Children 
                    ORDER BY FullName ASC";
                return await connection.QueryAsync<Child>(sql);
            }
        }

        public async Task<Child?> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                    SELECT 
                        ChildId,
                        ParentId,
                        FullName,
                        CAST(DateOfBirth AS DATE) AS DateOfBirth, -- Chỉ lấy phần ngày
                        Gender,
                        AdditionalInfo,
                        CreatedAt,
                        UpdatedAt
                    FROM Children 
                    WHERE ChildId = @Id";
                return await connection.QueryFirstOrDefaultAsync<Child>(sql, new { Id = id });
            }
        }

        public async Task<bool> UpdateAsync(Child child)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = @"
                UPDATE Children 
                SET ParentId = @ParentId, FullName = @FullName, DateOfBirth = @DateOfBirth, 
                    Gender = @Gender, AdditionalInfo = @AdditionalInfo, UpdatedAt = @UpdatedAt
                WHERE ChildId = @ChildId";

                var affectedRows = await connection.ExecuteAsync(sql, child);
                return affectedRows > 0;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var sql = "DELETE FROM Children WHERE ChildId = @Id";
                var affectedRows = await connection.ExecuteAsync(sql, new { Id = id });
                return affectedRows > 0;
            }
        }

        public Task<int> InsertAsyncc(Child obj)
        {
            throw new NotImplementedException();
        }
    }

    // Handler tùy chỉnh cho DateOnly
    public class DateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
        public override DateOnly Parse(object value)
        {
            if (value is DateTime dateTime)
            {
                return DateOnly.FromDateTime(dateTime);
            }
            if (value is string str && DateTime.TryParse(str, out var dt))
            {
                return DateOnly.FromDateTime(dt);
            }
            throw new FormatException($"Unable to parse '{value}' as DateOnly");
        }

        public override void SetValue(IDbDataParameter parameter, DateOnly value)
        {
            parameter.Value = value.ToString("yyyy-MM-dd");
        }
    }
}

