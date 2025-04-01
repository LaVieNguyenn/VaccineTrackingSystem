using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
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
    }
}
